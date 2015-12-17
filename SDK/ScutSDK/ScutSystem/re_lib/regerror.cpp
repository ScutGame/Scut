#include <stdio.h>

/* Added by Anuj Seth */
//#ifdef WIN32
#include <stdlib.h>
//#endif
/* End of addition */

void
regerror( char* s)
{
#ifdef ERRAVAIL
	error("regexp: %s", s);
#else
	fprintf(stderr, "regexp(3): %s", s);
	exit(1);
#endif
	/* NOTREACHED */
}
