/*
 * Definitions etc. for regexp(3) routines.
 *
 * Caveat:  this is V8 regexp(3) [actually, a reimplementation thereof],
 * not the System V one.
 */

#ifndef _REGEXP_H_
#define _REGEXP_H_

#define NSUBEXP  10
typedef struct regexp {
	const char *startp[NSUBEXP];
	const char *endp[NSUBEXP];
	char regstart;		/* Internal use only. */
	char reganch;		/* Internal use only. */
	char *regmust;		/* Internal use only. */
	int regmlen;		/* Internal use only. */
	char program[1];	/* Unwarranted chumminess with compiler. */
} regexp;

regexp *regcomp( char* re);
int regexec( regexp* r, const char* str, const char **s);
void regsub( regexp* r, char* str, char* substr);
void regerror( char* s);	/* for internal use only */
void set_regerror_func( void (*func)( char*));

#endif
