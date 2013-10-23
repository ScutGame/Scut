/****************************************************************************
Copyright (c) 2013-2015 Scutgame.com

http://www.scutgame.com

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
****************************************************************************/
#include "stdafx.h"
//#include <iostream>
#include "HttpSession.h"
#include "HttpClient.h"
#include "HttpClientResponse.h"
//#include "LogFile.h"

//extern LogFile     logFile;

using namespace ScutNetwork;


/*******************************************************************
* Function  : HttpSession constructor
* Parameters: -
* Returns   : -
* Purpose   : Initialize the object
*******************************************************************/
CHttpSession::CHttpSession()
{
	bSessionMgmt = FALSE;
	cookieHead   = NULL;
    cookieJar    = "";
}

/*******************************************************************
* Function  : HttpSession destructor
* Parameters: -
* Returns   : -
* Purpose   : Object cleanup
*******************************************************************/
CHttpSession::~CHttpSession()
{
	DeleteCookies();
}

/*******************************************************************
* Function  : SetCookies()
* Parameters: resp - The response object from which the cookies
*                    are to be extracted.
* Returns   : -
* Purpose   : Extract cookies
*******************************************************************/
void CHttpSession::AddCookie(const char *value)
{
	return; // Testing Curl CookieJar

	if(bSessionMgmt == FALSE)
	{
		/* Session management is not required */
		//logFile.Write(LOG_COOKIE, "session mgmt is off, not adding cookie '%s'.", value);
		return;
	}

	char cookie[4096];
	char *cookieStart = cookie;
	if (strlen(value) > sizeof(cookie)-1)
	{
		//logFile.Write(LOG_BASIC, "Cookie value greater than %d, not adding cookie '%s'.",sizeof(cookie)-1, value);
		return;
	}

	strcpy(cookie, value);

	char *ptr = strchr(cookie, ':');

	if(ptr != NULL)
	{
		while (*(++ptr) == ' ')
		{
			// Intentional empty loop
		}

		cookieStart = ptr;
	}

	/* 
	 * NOTE: The following can be a serious privacy issue.
	 * Since this class is currently being used only for
	 * YPOPs!, we are stripping out all the path
	 * information. So, this cookie will be sent for all
	 * requests to the Yahoo! mail server.
	 *
	 * TODO: Enhance session management to maintain a list
     * of cookies with relevant path information and send
	 * cookies for applicable domains only.
	 */

	ptr = strchr(cookieStart, ';');

	if(ptr != NULL)
	{
		*ptr = '\0';
	}

	ptr = strchr(cookieStart, '=');
		
	COOKIE *newCookie = new COOKIE;
	strncpy(newCookie->name, cookieStart, ptr - cookieStart);
	newCookie->name[ptr - cookieStart] = '\0';
	strcpy(newCookie->value, ptr + 1);
	newCookie->next = NULL;

	/* Check if this cookie already exists */
	COOKIE *cptr = cookieHead;
    COOKIE *prev = cptr;
	while(cptr != NULL)
	{
		if(strcmp(cptr->name, newCookie->name) == 0)
		{
			if (newCookie->value[0] == '\0')
            {
			    //logFile.Write(LOG_COOKIE, "deleting cookie/value '%s = %s'",cptr->name, cptr->value, value);
                if (cptr == cookieHead)
                {
                    cookieHead = cptr->next;
                }
                else
                {
                    prev->next = cptr->next;
                }
                delete cptr;
            }
            else
            {
                /* This is not a new cookie! */
			    strcpy(cptr->value, newCookie->value);
			    delete newCookie;
			    //logFile.Write(LOG_COOKIE, "replacing cookie value for '%s' to '%s' from string '%s'.",cptr->name, cptr->value, value);
            }
			return;
		}
        prev = cptr;
		cptr = cptr->next;
	}

	if(cookieHead == NULL)
		cookieHead = newCookie;
	else
		prev->next = newCookie; // prev points to the last one - this one becomes the tail

    //logFile.Write(LOG_COOKIE, "added cookie '%s' with value '%s' from string '%s'.",newCookie->name, newCookie->value, value);

}

/*******************************************************************
* Function  : GetCookies()
* Parameters: req - The request object needs to be populated with
*                   the cookies for this session.
* Returns   : -
* Purpose   : Set cookies
*******************************************************************/
CScutString CHttpSession::GetCookies(CHttpClient *req)
{
	return ""; //Testing Curl CookieJar

	if(bSessionMgmt == FALSE)
	{
		/* Session management is not required */
		//logFile.Write(LOG_ADVANCED, "session mgmt is off, not getting cookies.");
		return "";
	}

	CScutString cookies = "";
	COOKIE *ptr = cookieHead;

	while(ptr != NULL)
	{
		if(cookies.length() > 0)
			cookies += "; ";

		CScutString newCookie;
		newCookie.Format("%s=%s", ptr->name, ptr->value);
		cookies += newCookie;
		ptr = ptr->next;
		//logFile.Write(LOG_COOKIE, "added cookie to header: %s",newCookie.c_str());
	}

	return cookies;
}

// this routine is called from WebBrowser::SetSession right before we log into Yahoo
void CHttpSession::Initialize(const char *username)
{
    DeleteCookies();

    if (username && *username) 
	{
		cookieJar.Format("cookies-%s.txt",(username));
        //unlink(cookieJar.c_str()); // delete any old cookie file
	}
	else
	{
		cookieJar = "";
	}
    
    bSessionMgmt = TRUE;
}

void CHttpSession::DeleteCookies()
{
    // delete all cookies
	COOKIE *cptr = cookieHead;
	while(cptr != NULL)
	{
		//logFile.Write(LOG_COOKIE, "deleting cookie/value '%s = %s'",
		//			  cptr->name, cptr->value, value);
        cookieHead = cptr->next;
        delete cptr;
    }
}
