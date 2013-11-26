/*
www.sourceforge.net/projects/tinyxml
Original code (2.0 and earlier )copyright (c) 2000-2006 Lee Thomason (www.grinninglizard.com)

This software is provided 'as-is', without any express or implied
warranty. In no event will the authors be held liable for any
damages arising from the use of this software.

Permission is granted to anyone to use this software for any
purpose, including commercial applications, and to alter it and
redistribute it freely, subject to the following restrictions:

1. The origin of this software must not be misrepresented; you must
not claim that you wrote the original software. If you use this
software in a product, an acknowledgment in the product documentation
would be appreciated but is not required.

2. Altered source versions must be plainly marked as such, and
must not be misrepresented as being the original software.

3. This notice may not be removed or altered from any source
distribution.
*/

#include <ctype.h>

#ifdef TIXML_USE_STL
#include <sstream>
#include <iostream>
#endif

#include "tinyxml.h"


bool ScutTiXmlBase::condenseWhiteSpace = true;

// Microsoft compiler security
FILE* TiXmlFOpen( const char* filename, const char* mode )
{
	#if defined(_MSC_VER) && (_MSC_VER >= 1400 )
		FILE* fp = 0;
		errno_t err = fopen_s( &fp, filename, mode );
		if ( !err && fp )
			return fp;
		return 0;
	#else
		return fopen( filename, mode );
	#endif
}

void ScutTiXmlBase::EncodeString( const TIXML_STRING& str, TIXML_STRING* outString )
{
	int i=0;

	while( i<(int)str.length() )
	{
		unsigned char c = (unsigned char) str[i];

		if (    c == '&' 
		     && i < ( (int)str.length() - 2 )
			 && str[i+1] == '#'
			 && str[i+2] == 'x' )
		{
			// Hexadecimal character reference.
			// Pass through unchanged.
			// &#xA9;	-- copyright symbol, for example.
			//
			// The -1 is a bug fix from Rob Laveaux. It keeps
			// an overflow from happening if there is no ';'.
			// There are actually 2 ways to exit this loop -
			// while fails (error case) and break (semicolon found).
			// However, there is no mechanism (currently) for
			// this function to return an error.
			while ( i<(int)str.length()-1 )
			{
				outString->append( str.c_str() + i, 1 );
				++i;
				if ( str[i] == ';' )
					break;
			}
		}
		else if ( c == '&' )
		{
			outString->append( entity[0].str, entity[0].strLength );
			++i;
		}
		else if ( c == '<' )
		{
			outString->append( entity[1].str, entity[1].strLength );
			++i;
		}
		else if ( c == '>' )
		{
			outString->append( entity[2].str, entity[2].strLength );
			++i;
		}
		else if ( c == '\"' )
		{
			outString->append( entity[3].str, entity[3].strLength );
			++i;
		}
		else if ( c == '\'' )
		{
			outString->append( entity[4].str, entity[4].strLength );
			++i;
		}
		else if ( c < 32 )
		{
			// Easy pass at non-alpha/numeric/symbol
			// Below 32 is symbolic.
			char buf[ 32 ];
			
			#if defined(TIXML_SNPRINTF)		
				TIXML_SNPRINTF( buf, sizeof(buf), "&#x%02X;", (unsigned) ( c & 0xff ) );
			#else
				sprintf( buf, "&#x%02X;", (unsigned) ( c & 0xff ) );
			#endif		

			//*ME:	warning C4267: convert 'size_t' to 'int'
			//*ME:	Int-Cast to make compiler happy ...
			outString->append( buf, (int)strlen( buf ) );
			++i;
		}
		else
		{
			//char realc = (char) c;
			//outString->append( &realc, 1 );
			*outString += (char) c;	// somewhat more efficient function call.
			++i;
		}
	}
}


ScutTiXmlNode::ScutTiXmlNode( NodeType _type ) : ScutTiXmlBase()
{
	parent = 0;
	type = _type;
	firstChild = 0;
	lastChild = 0;
	prev = 0;
	next = 0;
}


ScutTiXmlNode::~ScutTiXmlNode()
{
	ScutTiXmlNode* node = firstChild;
	ScutTiXmlNode* temp = 0;

	while ( node )
	{
		temp = node;
		node = node->next;
		delete temp;
	}	
}


void ScutTiXmlNode::CopyTo( ScutTiXmlNode* target ) const
{
	target->SetValue (value.c_str() );
	target->userData = userData; 
}


void ScutTiXmlNode::Clear()
{
	ScutTiXmlNode* node = firstChild;
	ScutTiXmlNode* temp = 0;

	while ( node )
	{
		temp = node;
		node = node->next;
		delete temp;
	}	

	firstChild = 0;
	lastChild = 0;
}


ScutTiXmlNode* ScutTiXmlNode::LinkendChild( ScutTiXmlNode* node )
{
	assert( node->parent == 0 || node->parent == this );
	assert( node->GetDocument() == 0 || node->GetDocument() == this->GetDocument() );

	if ( node->Type() == ScutTiXmlNode::DOCUMENT )
	{
		delete node;
		if ( GetDocument() ) GetDocument()->SetError( TIXML_ERROR_DOCUMENT_TOP_ONLY, 0, 0, TIXML_ENCODING_UNKNOWN );
		return 0;
	}

	node->parent = this;

	node->prev = lastChild;
	node->next = 0;

	if ( lastChild )
		lastChild->next = node;
	else
		firstChild = node;			// it was an empty list.

	lastChild = node;
	return node;
}


ScutTiXmlNode* ScutTiXmlNode::InsertendChild( const ScutTiXmlNode& addThis )
{
	if ( addThis.Type() == ScutTiXmlNode::DOCUMENT )
	{
		if ( GetDocument() ) GetDocument()->SetError( TIXML_ERROR_DOCUMENT_TOP_ONLY, 0, 0, TIXML_ENCODING_UNKNOWN );
		return 0;
	}
	ScutTiXmlNode* node = addThis.Clone();
	if ( !node )
		return 0;

	return LinkendChild( node );
}


ScutTiXmlNode* ScutTiXmlNode::InsertBeforeChild( ScutTiXmlNode* beforeThis, const ScutTiXmlNode& addThis )
{	
	if ( !beforeThis || beforeThis->parent != this ) {
		return 0;
	}
	if ( addThis.Type() == ScutTiXmlNode::DOCUMENT )
	{
		if ( GetDocument() ) GetDocument()->SetError( TIXML_ERROR_DOCUMENT_TOP_ONLY, 0, 0, TIXML_ENCODING_UNKNOWN );
		return 0;
	}

	ScutTiXmlNode* node = addThis.Clone();
	if ( !node )
		return 0;
	node->parent = this;

	node->next = beforeThis;
	node->prev = beforeThis->prev;
	if ( beforeThis->prev )
	{
		beforeThis->prev->next = node;
	}
	else
	{
		assert( firstChild == beforeThis );
		firstChild = node;
	}
	beforeThis->prev = node;
	return node;
}


ScutTiXmlNode* ScutTiXmlNode::InsertAfterChild( ScutTiXmlNode* afterThis, const ScutTiXmlNode& addThis )
{
	if ( !afterThis || afterThis->parent != this ) {
		return 0;
	}
	if ( addThis.Type() == ScutTiXmlNode::DOCUMENT )
	{
		if ( GetDocument() ) GetDocument()->SetError( TIXML_ERROR_DOCUMENT_TOP_ONLY, 0, 0, TIXML_ENCODING_UNKNOWN );
		return 0;
	}

	ScutTiXmlNode* node = addThis.Clone();
	if ( !node )
		return 0;
	node->parent = this;

	node->prev = afterThis;
	node->next = afterThis->next;
	if ( afterThis->next )
	{
		afterThis->next->prev = node;
	}
	else
	{
		assert( lastChild == afterThis );
		lastChild = node;
	}
	afterThis->next = node;
	return node;
}


ScutTiXmlNode* ScutTiXmlNode::ReplaceChild( ScutTiXmlNode* replaceThis, const ScutTiXmlNode& withThis )
{
	if ( replaceThis->parent != this )
		return 0;

	ScutTiXmlNode* node = withThis.Clone();
	if ( !node )
		return 0;

	node->next = replaceThis->next;
	node->prev = replaceThis->prev;

	if ( replaceThis->next )
		replaceThis->next->prev = node;
	else
		lastChild = node;

	if ( replaceThis->prev )
		replaceThis->prev->next = node;
	else
		firstChild = node;

	delete replaceThis;
	node->parent = this;
	return node;
}


bool ScutTiXmlNode::RemoveChild( ScutTiXmlNode* removeThis )
{
	if ( removeThis->parent != this )
	{	
		assert( 0 );
		return false;
	}

	if ( removeThis->next )
		removeThis->next->prev = removeThis->prev;
	else
		lastChild = removeThis->prev;

	if ( removeThis->prev )
		removeThis->prev->next = removeThis->next;
	else
		firstChild = removeThis->next;

	delete removeThis;
	return true;
}

const ScutTiXmlNode* ScutTiXmlNode::FirstChild( const char * _value ) const
{
	const ScutTiXmlNode* node;
	for ( node = firstChild; node; node = node->next )
	{
		if ( strcmp( node->Value(), _value ) == 0 )
			return node;
	}
	return 0;
}


const ScutTiXmlNode* ScutTiXmlNode::LastChild( const char * _value ) const
{
	const ScutTiXmlNode* node;
	for ( node = lastChild; node; node = node->prev )
	{
		if ( strcmp( node->Value(), _value ) == 0 )
			return node;
	}
	return 0;
}


const ScutTiXmlNode* ScutTiXmlNode::IterateChildren( const ScutTiXmlNode* previous ) const
{
	if ( !previous )
	{
		return FirstChild();
	}
	else
	{
		assert( previous->parent == this );
		return previous->NextSibling();
	}
}


const ScutTiXmlNode* ScutTiXmlNode::IterateChildren( const char * val, const ScutTiXmlNode* previous ) const
{
	if ( !previous )
	{
		return FirstChild( val );
	}
	else
	{
		assert( previous->parent == this );
		return previous->NextSibling( val );
	}
}


const ScutTiXmlNode* ScutTiXmlNode::NextSibling( const char * _value ) const 
{
	const ScutTiXmlNode* node;
	for ( node = next; node; node = node->next )
	{
		if ( strcmp( node->Value(), _value ) == 0 )
			return node;
	}
	return 0;
}


const ScutTiXmlNode* ScutTiXmlNode::PreviousSibling( const char * _value ) const
{
	const ScutTiXmlNode* node;
	for ( node = prev; node; node = node->prev )
	{
		if ( strcmp( node->Value(), _value ) == 0 )
			return node;
	}
	return 0;
}


void ScutTiXmlElement::RemoveAttribute( const char * name )
{
    #ifdef TIXML_USE_STL
	TIXML_STRING str( name );
	ScutTiXmlAttribute* node = attributeSet.find( str );
	#else
	ScutTiXmlAttribute* node = attributeSet.find( name );
	#endif
	if ( node )
	{
		attributeSet.Remove( node );
		delete node;
	}
}

const ScutTiXmlElement* ScutTiXmlNode::FirstChildElement() const
{
	const ScutTiXmlNode* node;

	for (	node = FirstChild();
			node;
			node = node->NextSibling() )
	{
		if ( node->ToElement() )
			return node->ToElement();
	}
	return 0;
}


const ScutTiXmlElement* ScutTiXmlNode::FirstChildElement( const char * _value ) const
{
	const ScutTiXmlNode* node;

	for (	node = FirstChild( _value );
			node;
			node = node->NextSibling( _value ) )
	{
		if ( node->ToElement() )
			return node->ToElement();
	}
	return 0;
}


const ScutTiXmlElement* ScutTiXmlNode::NextSiblingElement() const
{
	const ScutTiXmlNode* node;

	for (	node = NextSibling();
			node;
			node = node->NextSibling() )
	{
		if ( node->ToElement() )
			return node->ToElement();
	}
	return 0;
}


const ScutTiXmlElement* ScutTiXmlNode::NextSiblingElement( const char * _value ) const
{
	const ScutTiXmlNode* node;

	for (	node = NextSibling( _value );
			node;
			node = node->NextSibling( _value ) )
	{
		if ( node->ToElement() )
			return node->ToElement();
	}
	return 0;
}


const ScutTiXmlDocument* ScutTiXmlNode::GetDocument() const
{
	const ScutTiXmlNode* node;

	for( node = this; node; node = node->parent )
	{
		if ( node->ToDocument() )
			return node->ToDocument();
	}
	return 0;
}


ScutTiXmlElement::ScutTiXmlElement (const char * _value)
	: ScutTiXmlNode( ScutTiXmlNode::ELEMENT )
{
	firstChild = lastChild = 0;
	value = _value;
}


#ifdef TIXML_USE_STL
ScutTiXmlElement::ScutTiXmlElement( const string& _value ) 
	: ScutTiXmlNode( ScutTiXmlNode::ELEMENT )
{
	firstChild = lastChild = 0;
	value = _value;
}
#endif


ScutTiXmlElement::ScutTiXmlElement( const ScutTiXmlElement& copy)
	: ScutTiXmlNode( ScutTiXmlNode::ELEMENT )
{
	firstChild = lastChild = 0;
	copy.CopyTo( this );	
}


void ScutTiXmlElement::operator=( const ScutTiXmlElement& base )
{
	ClearThis();
	base.CopyTo( this );
}


ScutTiXmlElement::~ScutTiXmlElement()
{
	ClearThis();
}


void ScutTiXmlElement::ClearThis()
{
	Clear();
	while( attributeSet.First() )
	{
		ScutTiXmlAttribute* node = attributeSet.First();
		attributeSet.Remove( node );
		delete node;
	}
}


const char* ScutTiXmlElement::Attribute( const char* name ) const
{
	const ScutTiXmlAttribute* node = attributeSet.find( name );
	if ( node )
		return node->Value();
	return 0;
}


#ifdef TIXML_USE_STL
const string* ScutTiXmlElement::Attribute( const string& name ) const
{
	const ScutTiXmlAttribute* node = attributeSet.find( name );
	if ( node )
		return &node->ValueStr();
	return 0;
}
#endif


const char* ScutTiXmlElement::Attribute( const char* name, int* i ) const
{
	const char* s = Attribute( name );
	if ( i )
	{
		if ( s ) {
			*i = atoi( s );
		}
		else {
			*i = 0;
		}
	}
	return s;
}


#ifdef TIXML_USE_STL
const string* ScutTiXmlElement::Attribute( const string& name, int* i ) const
{
	const string* s = Attribute( name );
	if ( i )
	{
		if ( s ) {
			*i = atoi( s->c_str() );
		}
		else {
			*i = 0;
		}
	}
	return s;
}
#endif


const char* ScutTiXmlElement::Attribute( const char* name, double* d ) const
{
	const char* s = Attribute( name );
	if ( d )
	{
		if ( s ) {
			*d = atof( s );
		}
		else {
			*d = 0;
		}
	}
	return s;
}


#ifdef TIXML_USE_STL
const string* ScutTiXmlElement::Attribute( const string& name, double* d ) const
{
	const string* s = Attribute( name );
	if ( d )
	{
		if ( s ) {
			*d = atof( s->c_str() );
		}
		else {
			*d = 0;
		}
	}
	return s;
}
#endif


int ScutTiXmlElement::QueryIntAttribute( const char* name, int* ival ) const
{
	const ScutTiXmlAttribute* node = attributeSet.find( name );
	if ( !node )
		return TIXML_NO_ATTRIBUTE;
	return node->QueryIntValue( ival );
}


#ifdef TIXML_USE_STL
int ScutTiXmlElement::QueryIntAttribute( const string& name, int* ival ) const
{
	const ScutTiXmlAttribute* node = attributeSet.find( name );
	if ( !node )
		return TIXML_NO_ATTRIBUTE;
	return node->QueryIntValue( ival );
}
#endif


int ScutTiXmlElement::QueryDoubleAttribute( const char* name, double* dval ) const
{
	const ScutTiXmlAttribute* node = attributeSet.find( name );
	if ( !node )
		return TIXML_NO_ATTRIBUTE;
	return node->QueryDoubleValue( dval );
}


#ifdef TIXML_USE_STL
int ScutTiXmlElement::QueryDoubleAttribute( const string& name, double* dval ) const
{
	const ScutTiXmlAttribute* node = attributeSet.find( name );
	if ( !node )
		return TIXML_NO_ATTRIBUTE;
	return node->QueryDoubleValue( dval );
}
#endif


void ScutTiXmlElement::SetAttribute( const char * name, int val )
{	
	char buf[64];
	#if defined(TIXML_SNPRINTF)		
		TIXML_SNPRINTF( buf, sizeof(buf), "%d", val );
	#else
		sprintf( buf, "%d", val );
	#endif
	SetAttribute( name, buf );
}


#ifdef TIXML_USE_STL
void ScutTiXmlElement::SetAttribute( const string& name, int val )
{	
   ostringstream oss;
   oss << val;
   SetAttribute( name, oss.str() );
}
#endif


void ScutTiXmlElement::SetDoubleAttribute( const char * name, double val )
{	
	char buf[256];
	#if defined(TIXML_SNPRINTF)		
		TIXML_SNPRINTF( buf, sizeof(buf), "%f", val );
	#else
		sprintf( buf, "%f", val );
	#endif
	SetAttribute( name, buf );
}


void ScutTiXmlElement::SetAttribute( const char * cname, const char * cvalue )
{
    #ifdef TIXML_USE_STL
	TIXML_STRING _name( cname );
	TIXML_STRING _value( cvalue );
	#else
	const char* _name = cname;
	const char* _value = cvalue;
	#endif

	ScutTiXmlAttribute* node = attributeSet.find( _name );
	if ( node )
	{
		node->SetValue( _value );
		return;
	}

	ScutTiXmlAttribute* attrib = new ScutTiXmlAttribute( cname, cvalue );
	if ( attrib )
	{
		attributeSet.Add( attrib );
	}
	else
	{
		ScutTiXmlDocument* document = GetDocument();
		if ( document ) document->SetError( TIXML_ERROR_OUT_OF_MEMORY, 0, 0, TIXML_ENCODING_UNKNOWN );
	}
}


#ifdef TIXML_USE_STL
void ScutTiXmlElement::SetAttribute( const string& name, const string& _value )
{
	ScutTiXmlAttribute* node = attributeSet.find( name );
	if ( node )
	{
		node->SetValue( _value );
		return;
	}

	ScutTiXmlAttribute* attrib = new ScutTiXmlAttribute( name, _value );
	if ( attrib )
	{
		attributeSet.Add( attrib );
	}
	else
	{
		ScutTiXmlDocument* document = GetDocument();
		if ( document ) document->SetError( TIXML_ERROR_OUT_OF_MEMORY, 0, 0, TIXML_ENCODING_UNKNOWN );
	}
}
#endif


void ScutTiXmlElement::Print( FILE* cfile, int depth ) const
{
	int i;
	assert( cfile );
	for ( i=0; i<depth; i++ ) {
		fprintf( cfile, "    " );
	}

	fprintf( cfile, "<%s", value.c_str() );

	const ScutTiXmlAttribute* attrib;
	for ( attrib = attributeSet.First(); attrib; attrib = attrib->Next() )
	{
		fprintf( cfile, " " );
		attrib->Print( cfile, depth );
	}

	// There are 3 different formatting approaches:
	// 1) An element without children is printed as a <foo /> node
	// 2) An element with only a text child is printed as <foo> text </foo>
	// 3) An element with children is printed on multiple lines.
	ScutTiXmlNode* node;
	if ( !firstChild )
	{
		fprintf( cfile, " />" );
	}
	else if ( firstChild == lastChild && firstChild->ToText() )
	{
		fprintf( cfile, ">" );
		firstChild->Print( cfile, depth + 1 );
		fprintf( cfile, "</%s>", value.c_str() );
	}
	else
	{
		fprintf( cfile, ">" );

		for ( node = firstChild; node; node=node->NextSibling() )
		{
			if ( !node->ToText() )
			{
				fprintf( cfile, "\n" );
			}
			node->Print( cfile, depth+1 );
		}
		fprintf( cfile, "\n" );
		for( i=0; i<depth; ++i ) {
			fprintf( cfile, "    " );
		}
		fprintf( cfile, "</%s>", value.c_str() );
	}
}


void ScutTiXmlElement::CopyTo( ScutTiXmlElement* target ) const
{
	// superclass:
	ScutTiXmlNode::CopyTo( target );

	// Element class: 
	// Clone the attributes, then clone the children.
	const ScutTiXmlAttribute* attribute = 0;
	for(	attribute = attributeSet.First();
	attribute;
	attribute = attribute->Next() )
	{
		target->SetAttribute( attribute->Name(), attribute->Value() );
	}

	ScutTiXmlNode* node = 0;
	for ( node = firstChild; node; node = node->NextSibling() )
	{
		target->LinkendChild( node->Clone() );
	}
}

bool ScutTiXmlElement::Accept( ScutTiXmlVisitor* visitor ) const
{
	if ( visitor->VisitEnter( *this, attributeSet.First() ) ) 
	{
		for ( const ScutTiXmlNode* node=FirstChild(); node; node=node->NextSibling() )
		{
			if ( !node->Accept( visitor ) )
				break;
		}
	}
	return visitor->VisitExit( *this );
}


ScutTiXmlNode* ScutTiXmlElement::Clone() const
{
	ScutTiXmlElement* clone = new ScutTiXmlElement( Value() );
	if ( !clone )
		return 0;

	CopyTo( clone );
	return clone;
}


const char* ScutTiXmlElement::GetText() const
{
	const ScutTiXmlNode* child = this->FirstChild();
	if ( child ) {
		const ScutTiXmlText* childText = child->ToText();
		if ( childText ) {
			return childText->Value();
		}
	}
	return 0;
}


ScutTiXmlDocument::ScutTiXmlDocument() : ScutTiXmlNode( ScutTiXmlNode::DOCUMENT )
{
	tabsize = 4;
	useMicrosoftBOM = false;
	ClearError();
}

ScutTiXmlDocument::ScutTiXmlDocument( const char * documentName ) : ScutTiXmlNode( ScutTiXmlNode::DOCUMENT )
{
	tabsize = 4;
	useMicrosoftBOM = false;
	value = documentName;
	ClearError();
}


#ifdef TIXML_USE_STL
ScutTiXmlDocument::ScutTiXmlDocument( const string& documentName ) : ScutTiXmlNode( ScutTiXmlNode::DOCUMENT )
{
	tabsize = 4;
	useMicrosoftBOM = false;
    value = documentName;
	ClearError();
}
#endif


ScutTiXmlDocument::ScutTiXmlDocument( const ScutTiXmlDocument& copy ) : ScutTiXmlNode( ScutTiXmlNode::DOCUMENT )
{
	copy.CopyTo( this );
}


void ScutTiXmlDocument::operator=( const ScutTiXmlDocument& copy )
{
	Clear();
	copy.CopyTo( this );
}


bool ScutTiXmlDocument::LoadFile( TiXmlEncoding encoding )
{
	// See STL_STRING_BUG below.
	//StringToBuffer buf( value );

	return LoadFile( Value(), encoding );
}


bool ScutTiXmlDocument::SaveFile() const
{
	// See STL_STRING_BUG below.
//	StringToBuffer buf( value );
//
//	if ( buf.buffer && SaveFile( buf.buffer ) )
//		return true;
//
//	return false;
	return SaveFile( Value() );
}

bool ScutTiXmlDocument::LoadFile( const char* _filename, TiXmlEncoding encoding )
{
	// There was a really terrifying little bug here. The code:
	//		value = filename
	// in the STL case, cause the assignment method of the string to
	// be called. What is strange, is that the string had the same
	// address as it's c_str() method, and so bad things happen. Looks
	// like a bug in the Microsoft STL implementation.
	// Add an extra string to avoid the crash.
	TIXML_STRING filename( _filename );
	value = filename;

	// reading in binary mode so that tinyxml can normalize the EOL
	FILE* file = TiXmlFOpen( value.c_str (), "rb" );	

	if ( file )
	{
		bool result = LoadFile( file, encoding );
		fclose( file );
		return result;
	}
	else
	{
		SetError( TIXML_ERROR_OPENING_FILE, 0, 0, TIXML_ENCODING_UNKNOWN );
		return false;
	}
}

bool ScutTiXmlDocument::LoadFile( FILE* file, TiXmlEncoding encoding )
{
	if ( !file ) 
	{
		SetError( TIXML_ERROR_OPENING_FILE, 0, 0, TIXML_ENCODING_UNKNOWN );
		return false;
	}

	// Delete the existing data:
	Clear();
	location.Clear();

	// Get the file size, so we can pre-allocate the string. HUGE speed impact.
	long length = 0;
	fseek( file, 0, SEEK_END );
	length = ftell( file );
	fseek( file, 0, SEEK_SET );

	// Strange case, but good to handle up front.
	if ( length <= 0 )
	{
		SetError( TIXML_ERROR_DOCUMENT_EMPTY, 0, 0, TIXML_ENCODING_UNKNOWN );
		return false;
	}

	// If we have a file, assume it is all one big XML file, and read it in.
	// The document parser may decide the document ends sooner than the entire file, however.
	TIXML_STRING data;
	data.reserve( length );

	// Subtle bug here. TinyXml did use fgets. But from the XML spec:
	// 2.11 end-of-Line Handling
	// <snip>
	// <quote>
	// ...the XML processor MUST behave as if it normalized all line breaks in external 
	// parsed entities (including the document entity) on input, before parsing, by translating 
	// both the two-character sequence #xD #xA and any #xD that is not followed by #xA to 
	// a single #xA character.
	// </quote>
	//
	// It is not clear fgets does that, and certainly isn't clear it works cross platform. 
	// Generally, you expect fgets to translate from the convention of the OS to the c/unix
	// convention, and not work generally.

	/*
	while( fgets( buf, sizeof(buf), file ) )
	{
		data += buf;
	}
	*/

	char* buf = new char[ length+1 ];
	buf[0] = 0;

	if ( fread( buf, length, 1, file ) != 1 ) {
		delete [] buf;
		SetError( TIXML_ERROR_OPENING_FILE, 0, 0, TIXML_ENCODING_UNKNOWN );
		return false;
	}

	const char* lastPos = buf;
	const char* p = buf;

	buf[length] = 0;
	while( *p ) {
		assert( p < (buf+length) );
		if ( *p == 0xa ) {
			// Newline character. No special rules for this. Append all the characters
			// since the last string, and include the newline.
			data.append( lastPos, (p-lastPos+1) );	// append, include the newline
			++p;									// move past the newline
			lastPos = p;							// and point to the new buffer (may be 0)
			assert( p <= (buf+length) );
		}
		else if ( *p == 0xd ) {
			// Carriage return. Append what we have so far, then
			// handle moving forward in the buffer.
			if ( (p-lastPos) > 0 ) {
				data.append( lastPos, p-lastPos );	// do not add the CR
			}
			data += (char)0xa;						// a proper newline

			if ( *(p+1) == 0xa ) {
				// Carriage return - new line sequence
				p += 2;
				lastPos = p;
				assert( p <= (buf+length) );
			}
			else {
				// it was followed by something else...that is presumably characters again.
				++p;
				lastPos = p;
				assert( p <= (buf+length) );
			}
		}
		else {
			++p;
		}
	}
	// Handle any left over characters.
	if ( p-lastPos ) {
		data.append( lastPos, p-lastPos );
	}		
	delete [] buf;
	buf = 0;

	Parse( data.c_str(), 0, encoding );

	if (  Error() )
        return false;
    else
		return true;
}


bool ScutTiXmlDocument::SaveFile( const char * filename ) const
{
	// The old c stuff lives on...
	FILE* fp = TiXmlFOpen( filename, "w" );
	if ( fp )
	{
		bool result = SaveFile( fp );
		fclose( fp );
		return result;
	}
	return false;
}


bool ScutTiXmlDocument::SaveFile( FILE* fp ) const
{
	if ( useMicrosoftBOM ) 
	{
		const unsigned char TIXML_UTF_LEAD_0 = 0xefU;
		const unsigned char TIXML_UTF_LEAD_1 = 0xbbU;
		const unsigned char TIXML_UTF_LEAD_2 = 0xbfU;

		fputc( TIXML_UTF_LEAD_0, fp );
		fputc( TIXML_UTF_LEAD_1, fp );
		fputc( TIXML_UTF_LEAD_2, fp );
	}
	Print( fp, 0 );
	return (ferror(fp) == 0);
}


void ScutTiXmlDocument::CopyTo( ScutTiXmlDocument* target ) const
{
	ScutTiXmlNode::CopyTo( target );

	target->error = error;
	target->errorId = errorId;
	target->errorDesc = errorDesc;
	target->tabsize = tabsize;
	target->errorLocation = errorLocation;
	target->useMicrosoftBOM = useMicrosoftBOM;

	ScutTiXmlNode* node = 0;
	for ( node = firstChild; node; node = node->NextSibling() )
	{
		target->LinkendChild( node->Clone() );
	}	
}


ScutTiXmlNode* ScutTiXmlDocument::Clone() const
{
	ScutTiXmlDocument* clone = new ScutTiXmlDocument();
	if ( !clone )
		return 0;

	CopyTo( clone );
	return clone;
}


void ScutTiXmlDocument::Print( FILE* cfile, int depth ) const
{
	assert( cfile );
	for ( const ScutTiXmlNode* node=FirstChild(); node; node=node->NextSibling() )
	{
		node->Print( cfile, depth );
		fprintf( cfile, "\n" );
	}
}


bool ScutTiXmlDocument::Accept( ScutTiXmlVisitor* visitor ) const
{
	if ( visitor->VisitEnter( *this ) )
	{
		for ( const ScutTiXmlNode* node=FirstChild(); node; node=node->NextSibling() )
		{
			if ( !node->Accept( visitor ) )
				break;
		}
	}
	return visitor->VisitExit( *this );
}


const ScutTiXmlAttribute* ScutTiXmlAttribute::Next() const
{
	// We are using knowledge of the sentinel. The sentinel
	// have a value or name.
	if ( next->value.empty() && next->name.empty() )
		return 0;
	return next;
}

/*
ScutTiXmlAttribute* ScutTiXmlAttribute::Next()
{
	// We are using knowledge of the sentinel. The sentinel
	// have a value or name.
	if ( next->value.empty() && next->name.empty() )
		return 0;
	return next;
}
*/

const ScutTiXmlAttribute* ScutTiXmlAttribute::Previous() const
{
	// We are using knowledge of the sentinel. The sentinel
	// have a value or name.
	if ( prev->value.empty() && prev->name.empty() )
		return 0;
	return prev;
}

/*
ScutTiXmlAttribute* ScutTiXmlAttribute::Previous()
{
	// We are using knowledge of the sentinel. The sentinel
	// have a value or name.
	if ( prev->value.empty() && prev->name.empty() )
		return 0;
	return prev;
}
*/

void ScutTiXmlAttribute::Print( FILE* cfile, int /*depth*/, TIXML_STRING* str ) const
{
	TIXML_STRING n, v;

	EncodeString( name, &n );
	EncodeString( value, &v );

	if (value.find ('\"') == TIXML_STRING::npos) {
		if ( cfile ) {
		fprintf (cfile, "%s=\"%s\"", n.c_str(), v.c_str() );
		}
		if ( str ) {
			(*str) += n; (*str) += "=\""; (*str) += v; (*str) += "\"";
		}
	}
	else {
		if ( cfile ) {
		fprintf (cfile, "%s='%s'", n.c_str(), v.c_str() );
		}
		if ( str ) {
			(*str) += n; (*str) += "='"; (*str) += v; (*str) += "'";
		}
	}
}


int ScutTiXmlAttribute::QueryIntValue( int* ival ) const
{
	if ( TIXML_SSCANF( value.c_str(), "%d", ival ) == 1 )
		return TIXML_SUCCESS;
	return TIXML_WRONG_TYPE;
}

int ScutTiXmlAttribute::QueryDoubleValue( double* dval ) const
{
	if ( TIXML_SSCANF( value.c_str(), "%lf", dval ) == 1 )
		return TIXML_SUCCESS;
	return TIXML_WRONG_TYPE;
}

void ScutTiXmlAttribute::SetIntValue( int _value )
{
	char buf [64];
	#if defined(TIXML_SNPRINTF)		
		TIXML_SNPRINTF(buf, sizeof(buf), "%d", _value);
	#else
		sprintf (buf, "%d", _value);
	#endif
	SetValue (buf);
}

void ScutTiXmlAttribute::SetDoubleValue( double _value )
{
	char buf [256];
	#if defined(TIXML_SNPRINTF)		
		TIXML_SNPRINTF( buf, sizeof(buf), "%lf", _value);
	#else
		sprintf (buf, "%lf", _value);
	#endif
	SetValue (buf);
}

int ScutTiXmlAttribute::IntValue() const
{
	return atoi (value.c_str ());
}

double  ScutTiXmlAttribute::DoubleValue() const
{
	return atof (value.c_str ());
}


ScutTiXmlComment::ScutTiXmlComment( const ScutTiXmlComment& copy ) : ScutTiXmlNode( ScutTiXmlNode::COMMENT )
{
	copy.CopyTo( this );
}


void ScutTiXmlComment::operator=( const ScutTiXmlComment& base )
{
	Clear();
	base.CopyTo( this );
}


void ScutTiXmlComment::Print( FILE* cfile, int depth ) const
{
	assert( cfile );
	for ( int i=0; i<depth; i++ )
	{
		fprintf( cfile,  "    " );
	}
	fprintf( cfile, "<!--%s-->", value.c_str() );
}


void ScutTiXmlComment::CopyTo( ScutTiXmlComment* target ) const
{
	ScutTiXmlNode::CopyTo( target );
}


bool ScutTiXmlComment::Accept( ScutTiXmlVisitor* visitor ) const
{
	return visitor->Visit( *this );
}


ScutTiXmlNode* ScutTiXmlComment::Clone() const
{
	ScutTiXmlComment* clone = new ScutTiXmlComment();

	if ( !clone )
		return 0;

	CopyTo( clone );
	return clone;
}


void ScutTiXmlText::Print( FILE* cfile, int depth ) const
{
	assert( cfile );
	if ( cdata )
	{
		int i;
		fprintf( cfile, "\n" );
		for ( i=0; i<depth; i++ ) {
			fprintf( cfile, "    " );
		}
		fprintf( cfile, "<![CDATA[%s]]>\n", value.c_str() );	// unformatted output
	}
	else
	{
		TIXML_STRING buffer;
		EncodeString( value, &buffer );
		fprintf( cfile, "%s", buffer.c_str() );
	}
}


void ScutTiXmlText::CopyTo( ScutTiXmlText* target ) const
{
	ScutTiXmlNode::CopyTo( target );
	target->cdata = cdata;
}


bool ScutTiXmlText::Accept( ScutTiXmlVisitor* visitor ) const
{
	return visitor->Visit( *this );
}


ScutTiXmlNode* ScutTiXmlText::Clone() const
{	
	ScutTiXmlText* clone = 0;
	clone = new ScutTiXmlText( "" );

	if ( !clone )
		return 0;

	CopyTo( clone );
	return clone;
}


ScutTiXmlDeclaration::ScutTiXmlDeclaration( const char * _version,
									const char * _encoding,
									const char * _standalone )
	: ScutTiXmlNode( ScutTiXmlNode::DECLARATION )
{
	version = _version;
	encoding = _encoding;
	standalone = _standalone;
}


#ifdef TIXML_USE_STL
ScutTiXmlDeclaration::ScutTiXmlDeclaration(	const string& _version,
									const string& _encoding,
									const string& _standalone )
	: ScutTiXmlNode( ScutTiXmlNode::DECLARATION )
{
	version = _version;
	encoding = _encoding;
	standalone = _standalone;
}
#endif


ScutTiXmlDeclaration::ScutTiXmlDeclaration( const ScutTiXmlDeclaration& copy )
	: ScutTiXmlNode( ScutTiXmlNode::DECLARATION )
{
	copy.CopyTo( this );	
}


void ScutTiXmlDeclaration::operator=( const ScutTiXmlDeclaration& copy )
{
	Clear();
	copy.CopyTo( this );
}


void ScutTiXmlDeclaration::Print( FILE* cfile, int /*depth*/, TIXML_STRING* str ) const
{
	if ( cfile ) fprintf( cfile, "<?xml " );
	if ( str )	 (*str) += "<?xml ";

	if ( !version.empty() ) {
		if ( cfile ) fprintf (cfile, "version=\"%s\" ", version.c_str ());
		if ( str ) { (*str) += "version=\""; (*str) += version; (*str) += "\" "; }
	}
	if ( !encoding.empty() ) {
		if ( cfile ) fprintf (cfile, "encoding=\"%s\" ", encoding.c_str ());
		if ( str ) { (*str) += "encoding=\""; (*str) += encoding; (*str) += "\" "; }
	}
	if ( !standalone.empty() ) {
		if ( cfile ) fprintf (cfile, "standalone=\"%s\" ", standalone.c_str ());
		if ( str ) { (*str) += "standalone=\""; (*str) += standalone; (*str) += "\" "; }
	}
	if ( cfile ) fprintf( cfile, "?>" );
	if ( str )	 (*str) += "?>";
}


void ScutTiXmlDeclaration::CopyTo( ScutTiXmlDeclaration* target ) const
{
	ScutTiXmlNode::CopyTo( target );

	target->version = version;
	target->encoding = encoding;
	target->standalone = standalone;
}


bool ScutTiXmlDeclaration::Accept( ScutTiXmlVisitor* visitor ) const
{
	return visitor->Visit( *this );
}


ScutTiXmlNode* ScutTiXmlDeclaration::Clone() const
{	
	ScutTiXmlDeclaration* clone = new ScutTiXmlDeclaration();

	if ( !clone )
		return 0;

	CopyTo( clone );
	return clone;
}


void ScutTiXmlUnknown::Print( FILE* cfile, int depth ) const
{
	for ( int i=0; i<depth; i++ )
		fprintf( cfile, "    " );
	fprintf( cfile, "<%s>", value.c_str() );
}


void ScutTiXmlUnknown::CopyTo( ScutTiXmlUnknown* target ) const
{
	ScutTiXmlNode::CopyTo( target );
}


bool ScutTiXmlUnknown::Accept( ScutTiXmlVisitor* visitor ) const
{
	return visitor->Visit( *this );
}


ScutTiXmlNode* ScutTiXmlUnknown::Clone() const
{
	ScutTiXmlUnknown* clone = new ScutTiXmlUnknown();

	if ( !clone )
		return 0;

	CopyTo( clone );
	return clone;
}


ScutTiXmlAttributeSet::ScutTiXmlAttributeSet()
{
	sentinel.next = &sentinel;
	sentinel.prev = &sentinel;
}


ScutTiXmlAttributeSet::~ScutTiXmlAttributeSet()
{
	assert( sentinel.next == &sentinel );
	assert( sentinel.prev == &sentinel );
}


void ScutTiXmlAttributeSet::Add( ScutTiXmlAttribute* addMe )
{
    #ifdef TIXML_USE_STL
	assert( !find( TIXML_STRING( addMe->Name() ) ) );	// Shouldn't be multiply adding to the set.
	#else
	assert( !find( addMe->Name() ) );	// Shouldn't be multiply adding to the set.
	#endif

	addMe->next = &sentinel;
	addMe->prev = sentinel.prev;

	sentinel.prev->next = addMe;
	sentinel.prev      = addMe;
}

void ScutTiXmlAttributeSet::Remove( ScutTiXmlAttribute* removeMe )
{
	ScutTiXmlAttribute* node;

	for( node = sentinel.next; node != &sentinel; node = node->next )
	{
		if ( node == removeMe )
		{
			node->prev->next = node->next;
			node->next->prev = node->prev;
			node->next = 0;
			node->prev = 0;
			return;
		}
	}
	assert( 0 );		// we tried to remove a non-linked attribute.
}


#ifdef TIXML_USE_STL
const ScutTiXmlAttribute* ScutTiXmlAttributeSet::find( const string& name ) const
{
	for( const ScutTiXmlAttribute* node = sentinel.next; node != &sentinel; node = node->next )
	{
		if ( node->name == name )
			return node;
	}
	return 0;
}

/*
ScutTiXmlAttribute*	ScutTiXmlAttributeSet::find( const string& name )
{
	for( ScutTiXmlAttribute* node = sentinel.next; node != &sentinel; node = node->next )
	{
		if ( node->name == name )
			return node;
	}
	return 0;
}
*/
#endif


const ScutTiXmlAttribute* ScutTiXmlAttributeSet::find( const char* name ) const
{
	for( const ScutTiXmlAttribute* node = sentinel.next; node != &sentinel; node = node->next )
	{
		if ( strcmp( node->name.c_str(), name ) == 0 )
			return node;
	}
	return 0;
}

/*
ScutTiXmlAttribute*	ScutTiXmlAttributeSet::find( const char* name )
{
	for( ScutTiXmlAttribute* node = sentinel.next; node != &sentinel; node = node->next )
	{
		if ( strcmp( node->name.c_str(), name ) == 0 )
			return node;
	}
	return 0;
}
*/

#ifdef TIXML_USE_STL	
istream& operator>> (istream & in, ScutTiXmlNode & base)
{
	TIXML_STRING tag;
	tag.reserve( 8 * 1000 );
	base.StreamIn( &in, &tag );

	base.Parse( tag.c_str(), 0, TIXML_DEFAULT_ENCODING );
	return in;
}
#endif


#ifdef TIXML_USE_STL	
ostream& operator<< (ostream & out, const ScutTiXmlNode & base)
{
	ScutTiXmlPrinter printer;
	printer.SetStreamPrinting();
	base.Accept( &printer );
	out << printer.Str();

	return out;
}


string& operator<< (string& out, const ScutTiXmlNode& base )
{
	ScutTiXmlPrinter printer;
	printer.SetStreamPrinting();
	base.Accept( &printer );
	out.append( printer.Str() );

	return out;
}
#endif


ScutTiXmlHandle ScutTiXmlHandle::FirstChild() const
{
	if ( node )
	{
		ScutTiXmlNode* child = node->FirstChild();
		if ( child )
			return ScutTiXmlHandle( child );
	}
	return ScutTiXmlHandle( 0 );
}


ScutTiXmlHandle ScutTiXmlHandle::FirstChild( const char * value ) const
{
	if ( node )
	{
		ScutTiXmlNode* child = node->FirstChild( value );
		if ( child )
			return ScutTiXmlHandle( child );
	}
	return ScutTiXmlHandle( 0 );
}


ScutTiXmlHandle ScutTiXmlHandle::FirstChildElement() const
{
	if ( node )
	{
		ScutTiXmlElement* child = node->FirstChildElement();
		if ( child )
			return ScutTiXmlHandle( child );
	}
	return ScutTiXmlHandle( 0 );
}


ScutTiXmlHandle ScutTiXmlHandle::FirstChildElement( const char * value ) const
{
	if ( node )
	{
		ScutTiXmlElement* child = node->FirstChildElement( value );
		if ( child )
			return ScutTiXmlHandle( child );
	}
	return ScutTiXmlHandle( 0 );
}


ScutTiXmlHandle ScutTiXmlHandle::Child( int count ) const
{
	if ( node )
	{
		int i;
		ScutTiXmlNode* child = node->FirstChild();
		for (	i=0;
				child && i<count;
				child = child->NextSibling(), ++i )
		{
			// nothing
		}
		if ( child )
			return ScutTiXmlHandle( child );
	}
	return ScutTiXmlHandle( 0 );
}


ScutTiXmlHandle ScutTiXmlHandle::Child( const char* value, int count ) const
{
	if ( node )
	{
		int i;
		ScutTiXmlNode* child = node->FirstChild( value );
		for (	i=0;
				child && i<count;
				child = child->NextSibling( value ), ++i )
		{
			// nothing
		}
		if ( child )
			return ScutTiXmlHandle( child );
	}
	return ScutTiXmlHandle( 0 );
}


ScutTiXmlHandle ScutTiXmlHandle::ChildElement( int count ) const
{
	if ( node )
	{
		int i;
		ScutTiXmlElement* child = node->FirstChildElement();
		for (	i=0;
				child && i<count;
				child = child->NextSiblingElement(), ++i )
		{
			// nothing
		}
		if ( child )
			return ScutTiXmlHandle( child );
	}
	return ScutTiXmlHandle( 0 );
}


ScutTiXmlHandle ScutTiXmlHandle::ChildElement( const char* value, int count ) const
{
	if ( node )
	{
		int i;
		ScutTiXmlElement* child = node->FirstChildElement( value );
		for (	i=0;
				child && i<count;
				child = child->NextSiblingElement( value ), ++i )
		{
			// nothing
		}
		if ( child )
			return ScutTiXmlHandle( child );
	}
	return ScutTiXmlHandle( 0 );
}


bool ScutTiXmlPrinter::VisitEnter( const ScutTiXmlDocument& )
{
	return true;
}

bool ScutTiXmlPrinter::VisitExit( const ScutTiXmlDocument& )
{
	return true;
}

bool ScutTiXmlPrinter::VisitEnter( const ScutTiXmlElement& element, const ScutTiXmlAttribute* firstAttribute )
{
	DoIndent();
	buffer += "<";
	buffer += element.Value();

	for( const ScutTiXmlAttribute* attrib = firstAttribute; attrib; attrib = attrib->Next() )
	{
		buffer += " ";
		attrib->Print( 0, 0, &buffer );
	}

	if ( !element.FirstChild() ) 
	{
		buffer += " />";
		DoLineBreak();
	}
	else 
	{
		buffer += ">";
		if (    element.FirstChild()->ToText()
			  && element.LastChild() == element.FirstChild()
			  && element.FirstChild()->ToText()->CDATA() == false )
		{
			simpleTextPrint = true;
			// no DoLineBreak()!
		}
		else
		{
			DoLineBreak();
		}
	}
	++depth;	
	return true;
}


bool ScutTiXmlPrinter::VisitExit( const ScutTiXmlElement& element )
{
	--depth;
	if ( !element.FirstChild() ) 
	{
		// nothing.
	}
	else 
	{
		if ( simpleTextPrint )
		{
			simpleTextPrint = false;
		}
		else
		{
			DoIndent();
		}
		buffer += "</";
		buffer += element.Value();
		buffer += ">";
		DoLineBreak();
	}
	return true;
}


bool ScutTiXmlPrinter::Visit( const ScutTiXmlText& text )
{
	if ( text.CDATA() )
	{
		DoIndent();
		buffer += "<![CDATA[";
		buffer += text.Value();
		buffer += "]]>";
		DoLineBreak();
	}
	else if ( simpleTextPrint )
	{
		TIXML_STRING str;
		ScutTiXmlBase::EncodeString( text.ValueTStr(), &str );
		buffer += str;
	}
	else
	{
		DoIndent();
		TIXML_STRING str;
		ScutTiXmlBase::EncodeString( text.ValueTStr(), &str );
		buffer += str;
		DoLineBreak();
	}
	return true;
}


bool ScutTiXmlPrinter::Visit( const ScutTiXmlDeclaration& declaration )
{
	DoIndent();
	declaration.Print( 0, 0, &buffer );
	DoLineBreak();
	return true;
}


bool ScutTiXmlPrinter::Visit( const ScutTiXmlComment& comment )
{
	DoIndent();
	buffer += "<!--";
	buffer += comment.Value();
	buffer += "-->";
	DoLineBreak();
	return true;
}


bool ScutTiXmlPrinter::Visit( const ScutTiXmlUnknown& unknown )
{
	DoIndent();
	buffer += "<";
	buffer += unknown.Value();
	buffer += ">";
	DoLineBreak();
	return true;
}

