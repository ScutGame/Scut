//
// Copyright (c) 2005, 2006 Wei Mingzhi <whistler@openoffice.org>
// All Rights Reserved.
//
// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License as
// published by the Free Software Foundation; either version 2 of
// the License, or (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA
// 02110-1301, USA
//

#include "iniEx.h"
#include "Trace.h"
namespace ScutDataLogic
{


void CIniFile::trim(char *str)
{
	int pos = 0;
	char *dest = str;

	// skip leading blanks
	while (str[pos] <= ' ' && str[pos] > 0)
		pos++;

	while (str[pos]) {
		*(dest++) = str[pos];
		pos++;
	}

	*(dest--) = '\0'; // store the null

	// remove trailing blanks
	while (dest >= str && *dest <= ' ' && *dest > 0)
		*(dest--) = '\0';
}

CIniFile::CIniFile():
ini(NULL), key_count(0), current_size(0)
{
#ifdef WITH_HASH
	m_Hash = (ini_key_t **)calloc(INI_HASH_KEY_SIZE, sizeof(ini_key_t *));
	if (m_Hash == NULL) {
		printf("Memory allocation error !\n");
		exit(1);
	}
#endif
}

CIniFile::CIniFile(const char *filename):
ini(NULL), key_count(0), current_size(0)
{
#ifdef WITH_HASH
	m_Hash = (ini_key_t **)calloc(INI_HASH_KEY_SIZE, sizeof(ini_key_t *));
	if (m_Hash == NULL) {
		printf("Memory allocation error !\n");
		exit(1);
	}
#endif
	Load(filename);
}

CIniFile::~CIniFile()
{
	FreeAllTheStuff();
#ifdef WITH_HASH
	free(m_Hash);
#endif
}

void CIniFile::FreeAllTheStuff()
{
	int i, j;

	if (ini) {
		// free all the memory allocated for this ini file
		for (i = 0; i < key_count; i++) {
			// delete all the values in this key...
			for (j = 0; j < ini[i].value_count; j++) {
				free(ini[i].values[j].value);
				free(ini[i].values[j].value_name);
			}

			free(ini[i].values);
			free(ini[i].key_name);
		}
		free(ini);
		ini = NULL;
	}

#ifdef WITH_HASH
	// null out the hash table
	memset(m_Hash, 0, sizeof(ini_key_t *) * INI_HASH_KEY_SIZE);
#endif
}

int CIniFile::Load(const char *filename)
{

	// open the model file
	FILE *fp = fopen(filename, "rb");


	//FILE *fp = fopen(filename, "r");

	if (fp == NULL) {
		ScutLog("error cannot load ini file %s\n", filename);
		return 1;
	}

	// if we already have a file loaded, free it first
	if (Valid()) {
		FreeAllTheStuff();
	}
//------------------->lrb
	//char str[256], section[256];
	 char str[300], section[256];
//<-------------------lrb
	section[0] = '\0';

	while (fgets(str, 300, fp)) {//while (fgets(str, 256, fp)) {
		trim(str); // trim all the blanks or linefeeds

		// skip all comment lines or empty lines
		if (!str[0] || str[0] == ';' || str[0] == '/' || str[0] == '#')
			continue;

		int length = strlen(str);
		char *p;
//-------------->lrb

//<--------------lrb
		// check if this is a session line (e.g., [SECTION])
		if (str[0] == '[' && str[length - 1] == ']') {
			strcpy(section, &str[1]);
			section[length - 2] = 0; // remove the ]

			trim(section); // trim section name after removing []
		} else if ((p = strchr(str, '=')) != NULL) {
			*(p++) = '\0';
			trim(str);
// /<--------------------lrb
// 			while((rt = strchr(p,'\\')) != NULL )
// 			{
// 				*rt='\n';
// 			}
// -------------------->lrb
			trim(p);
			Set(section, str, p);
		}
	}

	fclose(fp); // load completed; close the file
	return 0;
}

int CIniFile::Save(const char *filename)
{
	FILE *fp = fopen(filename, "w");

	if (fp == NULL) {
		printf("cannot save to INI file: %s\n", filename);
		return 1;
	}

	for (int i = 0; i < key_count; i++) {
		fprintf(fp, "[%s]\n", ini[i].key_name); // write the key name

		// write all the values...
		for (int j = 0; j < ini[i].value_count; j++) {
			fprintf(fp, "%s=%s\n", ini[i].values[j].value_name, ini[i].values[j].value);
		}

		fprintf(fp, "\n"); // add a line feed
	}

	fclose(fp); // save completed; close the file
	return 0;
}
void CIniFile::SetInt(const char *key, const char *value, int nValue)
{
	char szTemp[30] = {0};
	sprintf(szTemp, "%d", nValue);
	Set(key, value, szTemp);
}
// set the value in ini file
void CIniFile::Set(const char *key, const char *value, const char *set)
{
	ini_key_t *pKey = NULL;
	ini_value_t *pValue = NULL;
	int i;

#ifdef WITH_HASH
	// search if this key already exists...
	// first search in the hash table...
	unsigned short vhash = GetHashValue(key) % INI_HASH_KEY_SIZE;
	if (m_Hash[vhash] != NULL && strcasecmp(key, m_Hash[vhash]->key_name) == 0) {
		pKey = m_Hash[vhash]; // found this value in the hash table
	}
#endif
 
 	if (pKey == NULL) {
 		// not found in the hash table, do a normal search...
 		for (i = 0; i < key_count; i++) {
 			if (strcasecmp(ini[i].key_name, key) == 0) {
 				pKey = &ini[i];
 				break;
 			}
 		}
 	}

	// if this is a new key, try to allocate memory for it
	if (pKey == NULL) {
		key_count++;

		// if we don't have enough room for this new key, try to allocate more memory
		if (key_count > current_size) {
			current_size += INI_SIZE_INCREMENT;
			if (ini) {
				ini = (ini_key_t *)realloc(ini, sizeof(ini_key_t) * current_size);
			} else {
				ini = (ini_key_t *)malloc(sizeof(ini_key_t) * current_size);
			}

			if (!ini) {
				printf("Memory allocation error !");
				exit(1);
			}
		}

		pKey = &ini[key_count - 1];
		pKey->key_name = new char[strlen(key) + 1];
		strcpy(pKey->key_name,key);
		//pKey->key_name = strdup(key);
		pKey->values = NULL;
		pKey->value_count = 0;
		pKey->current_size = 0;
#ifdef WITH_HASH
		memset(pKey->hash, 0, sizeof(pKey->hash)); // zero out the hash table
		// store this new key in the hash table...
		unsigned short vhash = GetHashValue(pKey->key_name) % INI_HASH_KEY_SIZE;
		m_Hash[vhash] = pKey;
#endif
	}

#ifdef WITH_HASH
	// search if the value is already in the key...
	vhash = GetHashValue(value) % INI_HASH_VALUE_SIZE;
	if (pKey->hash[vhash] != NULL && strcasecmp(value, pKey->hash[vhash]->value_name) == 0) {
		pValue = pKey->hash[vhash]; // we have found the value in the hash table
	}
#endif

	if (pValue == NULL) {
		// value is not found in the hash table, do a normal search...
		for (i = 0; i < pKey->value_count; i++) {
			if (strcasecmp(value, pKey->values[i].value_name) == 0) {
				// we have found the value
				pValue = &pKey->values[i];
				break;
			}
		}
	}

	if (pValue != NULL) {
		// this value already exists in the key...
		free(pValue->value);
		//pValue->value = strdup(set);

		pValue->value = new char[strlen(set) + 1];
		strcpy(pValue->value,set);

		if (pValue->value == NULL) {
			printf("Memory allocation error !");
			exit(1);
		}
	} else {
		// this is a new value...
		pKey->value_count++;



		// if we don't have enough room for this new value, try to allocate more memory
		if (pKey->value_count > pKey->current_size) {
			pKey->current_size += INI_SIZE_INCREMENT;
			if (pKey->values) {
				pKey->values = (ini_value_t *)realloc(pKey->values, sizeof(ini_value_t) * pKey->current_size);
			} else {
				pKey->values = (ini_value_t *)malloc(sizeof(ini_value_t) * pKey->current_size);
			}

			if (pKey->values == NULL) {
				printf("Memory allocation error !");
				exit(1);
			}
		}

// 		pKey->values[pKey->value_count - 1].value_name = strdup(value);
// 		pKey->values[pKey->value_count - 1].value = strdup(set);

		pKey->values[pKey->value_count - 1].value_name = new char[strlen(value) + 1];
		strcpy(pKey->values[pKey->value_count - 1].value_name,value);
		pKey->values[pKey->value_count - 1].value = new char[strlen(set) + 1];
		strcpy(pKey->values[pKey->value_count - 1].value,set);
		

		if (pKey->values[pKey->value_count - 1].value == NULL ||
			pKey->values[pKey->value_count - 1].value_name == NULL) {
				printf("Memory allocation error !");
				exit(1);
		}

		trim(pKey->values[pKey->value_count - 1].value);
		trim(pKey->values[pKey->value_count - 1].value_name);

#ifdef WITH_HASH
		// store this new value in the hash table
		unsigned short vhash = GetHashValue(pKey->values[pKey->value_count - 1].value_name) % INI_HASH_VALUE_SIZE;
		pKey->hash[vhash] = &pKey->values[pKey->value_count - 1];
#endif
		
	}
}
int CIniFile::GetInt(const char *key, const char *value, const char *def)
{
	return atoi(Get(key, value, def));
}
// get a value from the ini file
const char *CIniFile::Get(const char *key, const char *value, const char *def)
{
	ini_key_t *pKey = NULL;
	int i;

#ifdef WITH_HASH
	// search for the key name...
	// first search in the hash table...
	unsigned short vhash = GetHashValue(key) % INI_HASH_KEY_SIZE;
	if (m_Hash[vhash] != NULL && strcasecmp(key, m_Hash[vhash]->key_name) == 0) {
		pKey = m_Hash[vhash]; // found this value in the hash table
	}
#endif

	if (pKey == NULL) {
		// Not found in the hash table, do a normal search...
		for (i = 0; i < key_count; i++) {
			if (strcasecmp(ini[i].key_name, key) == 0) {
				pKey = &ini[i];
				break;
			}
		}
	}

	if (pKey != NULL) {
#ifdef WITH_HASH
		// key found, search for the value in this key...
		vhash = GetHashValue(value) % INI_HASH_VALUE_SIZE;
		if (pKey->hash[vhash] != NULL && strcasecmp(value, pKey->hash[vhash]->value_name) == 0) {
			return pKey->hash[vhash]->value;
		}
#endif
		// not found in the hash table, do a normal linear search...
		for (i = 0; i < pKey->value_count; i++) {
			if (strcasecmp(pKey->values[i].value_name, value) == 0) {
				return pKey->values[i].value;
			}
		}
	}

	return def; // value is not found; use default value
}

#ifdef WITH_HASH
/**
* This hash function has been taken from an Article in Dr Dobbs Journal.
* This is normally a collision-free function, distributing keys evenly.
* Collision can be avoided by comparing the key itself in last resort.
*/
unsigned short CIniFile::GetHashValue(const char *sz)
{
	unsigned short hash = 0;

	while (*sz) {
		// convert all the characters to be upper case, so
		// that it will be case insensitive
		char a = *sz;
		if (a >= 'a' && a <= 'z') {
			a -= 'a' - 'A';
		}

		hash += (unsigned short)a;
		hash += (hash << 10);
		hash ^= (hash >> 6);

		sz++;
	}

	hash += (hash << 3);
	hash ^= (hash >> 11);
	hash += (hash << 15);

	return hash;
}
#endif

}
