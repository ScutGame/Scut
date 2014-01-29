package ccloader.lazyloaders{
    import ccloader.*;
    import ccloader.loadingtypes.*;
    import ccloader.lazyloaders.*;
    import flash.events.*;
    import flash.net.*;
    import flash.display.*;
    import flash.media.Sound;
    import flash.utils.*;
    import flash.system.LoaderContext;
    import flash.system.ApplicationDomain;
    import flash.media.SoundLoaderContext;
    /**
     *       @example Basic usage:<listing version="3.0">   
     var lazy : LazyJSONLoader = new LazyJSONLoader("sample-lazy.json", "myCCLoader");
    // listen to when the lazy loader has loaded the external definition
    lazy.addEventListener(Event.LAZY_LOADED, onLazyLoaded);
    // add regular events to the CCLoader instance
    lazy.addEventListener(ProgressEvent.PROGRESS, onLazyProgress);
    lazy.addEventListener(Event.LAZY_LOADED, onAllItemsLoaded);

    function onLazyLoaded(evt : Event) : void{
    // now you can add individual events for items
    onLazyLoaded.get("config").addEventListener(CCLoader.COMPLETE, onConfigLoaded);
    ...
    }
    </listing>
     */
    dynamic public class LazyJSONLoader extends LazyCCLoader {
        private var _decodeFunc : Function;
        function LazyJSONLoader(url : *, name : String, numConnections : int = CCLoader.DEFAULT_NUM_CONNECTIONS, logLevel : int = CCLoader.DEFAULT_LOG_LEVEL){
            super (url, name, numConnections, logLevel);
        }

        /** Reads a xml as a string and create a complete bulk loader from it.
         *   @param withData The xml to be read as a string.
         *   @private
         */

        public function get decodeFunc() : Function {
            if (!Boolean(_decodeFunc)){
                // defaults to adobe`s corelib decoder:
                var decoderClass : Object = getDefinitionByName("com.adobe.serialization.json.JSON");
                _decodeFunc = decoderClass.decode;
            } 
            return _decodeFunc; 
        }

        public function set decodeFunc(value:Function) : void { 
            _decodeFunc = value; 
        }

        lazy_loader override function _lazyParseLoader(withData : String) : void{
            var source : Object = decodeFunc(withData);
            stringSubstitutions = source["stringSubstitutions"] || undefined;
            _allowsAutoIDFromFileName = source["allowsAutoIDFromFileName"] || false;
            _numConnections = source["numConnections"] || CCLoader.DEFAULT_NUM_CONNECTIONS;
            logLevel = source["logLevel"] || CCLoader.DEFAULT_LOG_LEVEL;
            if (source["name"]){
                _name = source["name"];
            }
            var url : String;
            for each (var fileProp : Object in source["files"]) {
                var props : Object = fileProp;
                if (!String(props["url"])) {
                    trace("[LazyBulkLoader] got a item files with no url, ignoring");
                    continue;
                }
                if (props["context"]){
                    var context : Object;
                    if (CCLoader.guessType(String(fileProp["url"])) == CCLoader.TYPE_SOUND || fileProp["type"] == "sound") {
                        context = new SoundLoaderContext();
                    } else {
                        context = new LoaderContext();
                    }
                    context.applicationDomain = ApplicationDomain.currentDomain;
                    props[CCLoader.CONTEXT] = context;
                }else if (fileProp["headers"]){
                    var oldHeaders  : Object = fileProp["headers"];
                    fileProp["headers"]= [];
                    for each(var headerObject : Object in oldHeaders){
                        for (var headerName : String in headerObject){
                            var theHeader : URLRequestHeader = new URLRequestHeader(headerName, headerObject[headerName]);
                            fileProp["headers"].push(theHeader);
                        }
                    }
                }
                url = props["url"];
                delete props["url"];
                var theItem : LoadingItem = add(url, props);
            }
        }
    }
}

