# cocos2d-console



## Download

```sh
$ NOT DONE YET
```

## Install

```sh
$ NOT DONE YET
```

## Vision of cocos2d-console


A command line tool that lets you create, run, publish, debug, etc… your game. It is the swiss-army knife for cocos2d.

This command line tool is in its early stages.

Examples:

```
# starts a new project called "My Game" for iOS and Android with JS Bindigns
$ cocos2d new "My Game" --mobile --js

$ cd "My Game"

# Will compile the source code, publish the assests, and then it will send the binary to the simulator
$ cocos2d run --ios


# Will generate a distribution .ipa file ready to be summitted to the App Store
$ cocos2d dist --ios


# Will generate published files
$ cocos2d.py publish --ios

# Will generate bytecode files
$ cocos2d jscompile -d output_dir -s cocos2dx_root/scripting/javascript/bindings/js -s cocos2dx_root/samples/Javascript/Shared/tests -o game.min.js -j compiler_config_sample.json -c

```

# Devel Info

## Internals

`cocos2d.py` is an script whose only responsability is to call its plugins.

eg:
```
// It will just print all the registered plugins
$ python cocos2d.py
```

```
// It will call the "new" plugin 
$ python cocos2d.py new
``` 

```
// It will call the "dist" plugin with the --help argument 
$ python cocos2d.py dist --help
``` 


## Adding new plugin to the console

You have to edit the `cocos2d.ini` file, and add your new plugin there.

Let's say that you want to add a plugin that minifies JS code.

```
# Adds the minify_js plugin
[plugin "minify_js"]
# should be a subclass of CCPlugin
class = cocos2d_minify_js.CCPluginMinifyJS
``` 

And now you have to create a file called `cocos2d_minify_js.py` with the following structure.

```python
import cocos2d

# Plugins should be a sublass of CCPlugin
class CCPluginMinifyJS(cocos2d.CCPlugin):

    @staticmethod
    def brief_description():
        return "minify_js\t\tminifies JS code"

    def run(self, argv):
        print "plugin called!"
        print argv
```
