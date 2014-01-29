package ccloader.loadingtypes {

    import ccloader.CCLoader;

    import flash.display.*;
    import flash.events.*;
    import flash.net.*;
    import flash.utils.*;

    /** @private */
    public class ImageItem extends LoadingItem {

        public var loader : Loader;

        public function ImageItem(url : URLRequest, type : String, uid : String){
            specificAvailableProps = [CCLoader.CONTEXT];
            super(url, type, uid);
        }

        override public function _parseOptions(props : Object)  : Array{
            _context = props[CCLoader.CONTEXT] || null;

            return super._parseOptions(props);
        }

        override public function load() : void{
            super.load();
            loader = new Loader();
            loader.contentLoaderInfo.addEventListener(ProgressEvent.PROGRESS, onProgressHandler, false, 0, true);
            loader.contentLoaderInfo.addEventListener(Event.COMPLETE, onCompleteHandler, false, 0, true);
            loader.contentLoaderInfo.addEventListener(Event.INIT, onInitHandler, false, 0, true);
            loader.contentLoaderInfo.addEventListener(IOErrorEvent.IO_ERROR, onErrorHandler, false, 100, true);
            loader.contentLoaderInfo.addEventListener(SecurityErrorEvent.SECURITY_ERROR, onSecurityErrorHandler, false, 0, true);
            loader.contentLoaderInfo.addEventListener(Event.OPEN, onStartedHandler, false, 0, true);  
            loader.contentLoaderInfo.addEventListener(HTTPStatusEvent.HTTP_STATUS, super.onHttpStatusHandler, false, 0, true);
            try{
                // TODO: test for security error thown.
                loader.load(url, _context);
            }catch( e : SecurityError){
                onSecurityErrorHandler(_createErrorEvent(e));
            }

        };

        public function _onHttpStatusHandler(evt : HTTPStatusEvent) : void{
            _httpStatus = evt.status;
            dispatchEvent(evt);
        }

        override public function onErrorHandler(evt : ErrorEvent) : void{
            super.onErrorHandler(evt);
        }

        public function onInitHandler(evt : Event) :void{
            dispatchEvent(evt);
        }

        override public function onCompleteHandler(evt : Event) : void {
            // TODO: test for the different behaviour when loading items with 
            // the a specific crossdomain and without one
            try{
                // of no crossdomain has allowed this operation, this might
                // raise a security error
                _content = loader.content;
                super.onCompleteHandler(evt);
            }catch(e : SecurityError){
                // we can still use the Loader object (no dice for accessing it as data
                // though. Oh boy:
                _content = loader;
                super.onCompleteHandler(evt);
                // I am really unsure whether I should throw this event
                // it would be nice, but simply delegating the error handling to user's code 
                // seems cleaner (and it also mimics the Standar API behaviour on this respect)
                //onSecurityErrorHandler(e);
            }

        };

        override public function stop() : void{
            try{
                if(loader){
                    loader.close();
                }
            }catch(e : Error){

            }
            super.stop();
        };

        /** Gets a  definition from a fully qualified path (can be a Class, function or namespace). 
          @param className The fully qualified class name as a string.
          @return The definition object with that name or null of not found.
         */
        public function getDefinitionByName(className : String) : Object{
            if (loader.contentLoaderInfo.applicationDomain.hasDefinition(className)){
                return loader.contentLoaderInfo.applicationDomain.getDefinition(className);
            }
            return null;
        }

        override public function cleanListeners() : void {
            if (loader){
                var removalTarget : Object = loader.contentLoaderInfo;
                removalTarget.removeEventListener(ProgressEvent.PROGRESS, onProgressHandler, false);
                removalTarget.removeEventListener(Event.COMPLETE, onCompleteHandler, false);
                removalTarget.removeEventListener(Event.INIT, onInitHandler, false);
                removalTarget.removeEventListener(IOErrorEvent.IO_ERROR, onErrorHandler, false);
                removalTarget.removeEventListener(CCLoader.OPEN, onStartedHandler, false);
                removalTarget.removeEventListener(HTTPStatusEvent.HTTP_STATUS, super.onHttpStatusHandler, false);
            }

        }

        override public function isImage(): Boolean{
            return (type == CCLoader.TYPE_IMAGE);
        }

        override public function isSWF(): Boolean{
            return (type == CCLoader.TYPE_MOVIECLIP);
        }


        override public function isStreamable() : Boolean{
            return isSWF();
        }

        override public function destroy() : void{
            stop();
            cleanListeners();
            _content = null;
            // this is an player 10 only feature. as such we must check it's existence
            // with the array acessor, or else the compiler will barf on player 9
            if (loader && loader.hasOwnProperty("unloadAndStop") && loader["unloadAndStop"] is Function) {
                loader["unloadAndStop"](true);
            }else if (loader && loader.hasOwnProperty("unload") && loader["unload"] is Function) {
                // this is an air only feature. as such we must check it's existence
                // with the array acessor, or else the compiler will barf on non air projects
                loader["unload"]();
            }


            loader = null;
        }

    }}
