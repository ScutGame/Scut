#!/usr/bin/python
# ----------------------------------------------------------------------------
# cocos2d "new" plugin
#
# Author: Ricardo Quesada
# Copyright 2013 (C) Zynga, Inc
#
# License: MIT
# ----------------------------------------------------------------------------
'''
"new" plugin for cocos2d command line tool
'''

__docformat__ = 'restructuredtext'

# python
import os
import sys
import getopt
import ConfigParser

import cocos2d


def help():
    sys.exit(-1)


#
# Plugins should be a sublass of CCJSPlugin
#
class CCPluginNew(cocos2d.CCPlugin):

    @staticmethod
    def brief_description():
        return "new\t\tcreates a new project"

    # parse arguments
    def parse_args(self, argv):
        name = directory = argv[0]
        try:
            opts, args = getopt.getopt(argv, "d:h", ["dir=", "help"])

            for opt, arg in opts:
                if opt in ("-d", "--dir"):
                    directory = arg
                elif opt in ("h", "--help"):
                    help()
        except getopt.GetoptError, e:
            print e
            opts, args = getopt.getopt(argv, "", [])

        return {"dir": directory, "name": name}

    # create the project
    def create_project(self, args):
        #1st: create directory
        d = os.path.join(os.path.abspath('.'), args["dir"])
        if os.path.exists(d):
            raise Exception("Error: Can't create project. Directory already exists: %s" % d)
        print '*****: %s' % args["dir"]
        os.makedirs(d)

        # 2nd create ini file
        config = ConfigParser.RawConfigParser()
        project_name = 'project "%s"' % args["name"]
        config.add_section(project_name)
        config.set(project_name, 'res_path', 'res')
        config.set(project_name, 'js_path', 'js')

        # Writing our configuration file to 'example.cfg'
        with open(os.path.join(d, 'config.ini'), 'wb') as configfile:
            config.write(configfile)

        # 3rd create default directories

    # main entry point
    def run(self, argv):
        args = self.parse_args(argv)
        self.create_project(args)

