#!/usr/bin/python
# ----------------------------------------------------------------------------
# cocos2d "version" plugin
#
# Author: Ricardo Quesada
# Copyright 2013 (C) Zynga, Inc
#
# License: MIT
# ----------------------------------------------------------------------------
'''
"version" plugin for cocos2d command line tool
'''

__docformat__ = 'restructuredtext'


# python
import cocos2d


#
# Plugins should be a sublass of CCJSPlugin
#
class CCPluginVersion(cocos2d.CCPlugin):

    @staticmethod
    def brief_description():
        return "version\t\tprints the version of the installed components"

    def run(self, argv):
        print "version called!"
        print argv
