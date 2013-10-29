#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <sys/ioctl.h>
#include <sys/types.h>
#include <sys/socket.h>
#include <netinet/in.h>
#include <netdb.h>
#include <arpa/inet.h>
#include <sys/sockio.h>
#include <net/if.h>
#include <errno.h>
#include <net/if_dl.h>
#include <unistd.h>
#include "IPAddress.h"
#include <sys/sysctl.h>
#include <string.h>
//#include <net/if_types.h>
#include <ifaddrs.h>
#define IFT_ETHER       0x6
#define	min(a,b)	((a) < (b) ? (a) : (b))
#define	max(a,b)	((a) > (b) ? (a) : (b))

#define BUFFERSIZE	4000

char *if_names[MAXADDRS];
char *ip_names[MAXADDRS];
char *hw_addrs[MAXADDRS];
unsigned long ip_addrs[MAXADDRS];

static int   nextAddr = 0;

void InitAddresses()
{
	int i;
	for (i=0; i<MAXADDRS; ++i)
	{
		if_names[i] = ip_names[i] = hw_addrs[i] = NULL;
		ip_addrs[i] = 0;
	}
}

void FreeAddresses()
{
	int i;
	for (i=0; i<MAXADDRS; ++i)
	{
		if (if_names[i] != 0) free(if_names[i]);
		if (ip_names[i] != 0) free(ip_names[i]);
		if (hw_addrs[i] != 0) free(hw_addrs[i]);
		ip_addrs[i] = 0;
	}
	InitAddresses();
}

void GetIPAddresses()
{
	int                 i, len, flags;
	char                buffer[BUFFERSIZE], *ptr, lastname[IFNAMSIZ], *cptr;
	struct ifconf       ifc;
	struct ifreq        *ifr, ifrcopy;
	struct sockaddr_in	*sin;

	char temp[80];

	int sockfd;

	for (i=0; i<MAXADDRS; ++i)
	{
		if_names[i] = ip_names[i] = NULL;
		ip_addrs[i] = 0;
	}

	sockfd = socket(AF_INET, SOCK_DGRAM, 0);
	if (sockfd < 0)
	{
		perror("socket failed");
		return;
	}
	
	ifc.ifc_len = BUFFERSIZE;
	ifc.ifc_buf = buffer;
	
	if (ioctl(sockfd, SIOCGIFCONF, &ifc) < 0)
	{
		perror("ioctl error");
		return;
	}
	
	lastname[0] = 0;
	
	for (ptr = buffer; ptr < buffer + ifc.ifc_len; )
	{
		ifr = (struct ifreq *)ptr;
		len = max(sizeof(struct sockaddr), ifr->ifr_addr.sa_len);
		ptr += sizeof(ifr->ifr_name) + len;	// for next one in buffer
	
		if (ifr->ifr_addr.sa_family != AF_INET)
		{
			continue;	// ignore if not desired address family
		}
	
		if ((cptr = (char *)strchr(ifr->ifr_name, ':')) != NULL)
		{
			*cptr = 0;		// replace colon will null
		}
	
		if (strncmp(lastname, ifr->ifr_name, IFNAMSIZ) == 0)
		{
			continue;	/* already processed this interface */
		}
	
		memcpy(lastname, ifr->ifr_name, IFNAMSIZ);
	
		ifrcopy = *ifr;
		ioctl(sockfd, SIOCGIFFLAGS, &ifrcopy);
		flags = ifrcopy.ifr_flags;
		if ((flags & IFF_UP) == 0)
		{
			continue;	// ignore if interface not up
		}
	
		if_names[nextAddr] = (char *)malloc(strlen(ifr->ifr_name)+1);
		if (if_names[nextAddr] == NULL)
		{
			return;
		}
		strcpy(if_names[nextAddr], ifr->ifr_name);
	
		sin = (struct sockaddr_in *)&ifr->ifr_addr;
		strcpy(temp, inet_ntoa(sin->sin_addr));
	
		ip_names[nextAddr] = (char *)malloc(strlen(temp)+1);
		if (ip_names[nextAddr] == NULL)
		{
			return;
		}
		strcpy(ip_names[nextAddr], temp);

		ip_addrs[nextAddr] = sin->sin_addr.s_addr;

		++nextAddr;
	}
	
	close(sockfd);
}

void GetHWAddresses()
{
   struct ifconf ifc;
   struct ifreq *ifr;
   int i, sockfd;
   char buffer[BUFFERSIZE], *cp, *cplim;
   char temp[80];

   for (i=0; i<MAXADDRS; ++i)
   {
      hw_addrs[i] = NULL;
   }

   sockfd = socket(AF_INET, SOCK_DGRAM, 0);
   if (sockfd < 0)
   {
      perror("socket failed");
      return;
   }

   ifc.ifc_len = BUFFERSIZE;
   ifc.ifc_buf = buffer;

   if (ioctl(sockfd, SIOCGIFCONF, (char *)&ifc) < 0)
   {
      perror("ioctl error");
      close(sockfd);
	  return;
   }

   ifr = ifc.ifc_req;

   cplim = buffer + ifc.ifc_len;

   for (cp=buffer; cp < cplim; )
   {
      ifr = (struct ifreq *)cp;
	  if (ifr->ifr_addr.sa_family == AF_LINK)
	  {
	     struct sockaddr_dl *sdl = (struct sockaddr_dl *)&ifr->ifr_addr;
		 int a,b,c,d,e,f;
		 int i;

		 strcpy(temp, (char *)ether_ntoa(LLADDR(sdl)));
		 sscanf(temp, "%x:%x:%x:%x:%x:%x", &a, &b, &c, &d, &e, &f);
         sprintf(temp, "%02X:%02X:%02X:%02X:%02X:%02X",a,b,c,d,e,f);

		 for (i=0; i<MAXADDRS; ++i)
		 {
		    if ((if_names[i] != NULL) && (strcmp(ifr->ifr_name, if_names[i]) == 0))
			{
			   if (hw_addrs[i] == NULL)
			   {
   			      hw_addrs[i] = (char *)malloc(strlen(temp)+1);
				  strcpy(hw_addrs[i], temp);
				  break;
			   }
			}
		}
	  }
      cp += sizeof(ifr->ifr_name) + max(sizeof(ifr->ifr_addr), ifr->ifr_addr.sa_len);
   }

   close(sockfd);
}

char*  getMacAddressIOS(char* macAddress, char* ifName) {
	
	int  success;
	struct ifaddrs * addrs;
	struct ifaddrs * cursor;
	const struct sockaddr_dl * dlAddr;
	const unsigned char* base;
	int i;
	
	success = getifaddrs(&addrs) == 0;
	if (success) {
		cursor = addrs;
		while (cursor != 0) {
			if ( (cursor->ifa_addr->sa_family == AF_LINK)
				&& (((const struct sockaddr_dl *) cursor->ifa_addr)->sdl_type == IFT_ETHER) && strcmp(ifName,  cursor->ifa_name)==0 ) {
				dlAddr = (const struct sockaddr_dl *) cursor->ifa_addr;
				base = (const unsigned char*) &dlAddr->sdl_data[dlAddr->sdl_nlen];
				strcpy(macAddress, ""); 
				for (i = 0; i < dlAddr->sdl_alen; i++) {
					if (i != 0) {
						strcat(macAddress, ":");
					}
					char partialAddr[3];
					sprintf(partialAddr, "%02X", base[i]);
					strcat(macAddress, partialAddr);
					
				}
			}
			cursor = cursor->ifa_next;
		}
		
		freeifaddrs(addrs);
	}    
	return macAddress;
}
