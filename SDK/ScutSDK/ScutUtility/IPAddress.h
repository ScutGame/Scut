/*
 *  IPAddress.h
 *  PersonalProxy
 *
 *  Created by Chris Whiteford on 2009-02-20.
 *  Copyright 2009 __MyCompanyName__. All rights reserved.
 *
 */

#define MAXADDRS	32

extern char *if_names[MAXADDRS];
extern char *ip_names[MAXADDRS];
extern char *hw_addrs[MAXADDRS];
extern unsigned long ip_addrs[MAXADDRS];

// Function prototypes
void InitAddresses();
void FreeAddresses();
void GetIPAddresses();
void GetHWAddresses();
char*  getMacAddressIOS(char* macAddress, char* ifName);