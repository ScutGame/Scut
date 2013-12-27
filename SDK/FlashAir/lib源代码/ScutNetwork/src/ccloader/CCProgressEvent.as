package ccloader{
    import flash.events.*;

    /**
     *	An event that holds information about the status of a <code>CCLoader</code>.
     *  
     *   As this event subclasses <code>ProgressEvent</code>, you can choose to listen to <code>CCProgressEvent</code> or <code>ProgressEvent</code> instances, but this class provides more useful information about loading status.
     *  
     *	@langversion ActionScript 3.0
     *	@playerversion Flash 9.0
     *
     *	@author Arthur Debert
     *	@since  15.09.2007
     */
    public class CCProgressEvent extends ProgressEvent {
        /* The name of this event */
        public static const PROGRESS : String = "progress";
        public static const COMPLETE : String = "complete";

        /** How many bytes have loaded so far.*/
        public var bytesTotalCurrent : int;
        /** @private */
        public var _ratioLoaded : Number;
        /** @private */

        public var _percentLoaded : Number;
        /** @private */
        public var _weightPercent : Number;
        /** Number of items already loaded */
        public var itemsLoaded : int;
        /** Number of items to be loaded */
        public var itemsTotal : int;

        public var name : String;

        public function CCProgressEvent( name : String, bubbles:Boolean=true, cancelable:Boolean=false ){
            super(name, bubbles, cancelable);		
            this.name = name;
        }

        /** Sets loading information.*/
        public function setInfo(
                bytesLoaded : int ,
                bytesTotal : int,
                bytesTotalCurrent : int, 
                itemsLoaded : int ,
                itemsTotal : int,
                weightPercent : Number
                ): void{
            this.bytesLoaded = bytesLoaded;
            this.bytesTotal = bytesTotal;
            this.bytesTotalCurrent = bytesTotalCurrent;
            this.itemsLoaded = itemsLoaded;
            this.itemsTotal = itemsTotal;
            this.weightPercent = weightPercent;
            this.percentLoaded = bytesTotal > 0 ? (bytesLoaded / bytesTotal) : 0;
            ratioLoaded = itemsTotal == 0 ? 0 : itemsLoaded / itemsTotal;
        }

        /* Returns an identical copy of this object
         *   @return A cloned instance of this object.
         */
        override public function clone() : Event {
            var b : CCProgressEvent = new CCProgressEvent(name, bubbles, cancelable);
            b.setInfo(bytesLoaded, bytesTotal, bytesTotalCurrent, itemsLoaded, itemsTotal, weightPercent);
            return b;	
        }

        /** Returns a <code>String</code> will all available information for this event.
         * @return A <code>String</code> will loading information.
         */
        public function loadingStatus () : String{
            var names : Array = [];
            names.push("bytesLoaded: " + bytesLoaded);
            names.push("bytesTotal: " + bytesTotal);
            names.push("itemsLoaded: " + itemsLoaded);
            names.push("itemsTotal: " + itemsTotal);
            names.push("bytesTotalCurrent: " + bytesTotalCurrent);
            names.push("percentLoaded: " + CCLoader.truncateNumber(percentLoaded));
            names.push("weightPercent: " + CCLoader.truncateNumber(weightPercent));
            names.push("ratioLoaded: " + CCLoader.truncateNumber(ratioLoaded));
            return "CCProgressEvent " + names.join(", ") + ";";
        }

        /** A number between 0 - 1 that indicates progress regarding weights */
        public function get weightPercent() : Number { 
            return truncateToRange(_weightPercent); 
        }


        public function set weightPercent(value:Number) : void { 
            if (isNaN(value) || !isFinite(value)) {
                value = 0;		}
            _weightPercent = value; 
        }

        /** A number between 0 - 1 that indicates progress regarding bytes */
        public function get percentLoaded() : Number { 
            return truncateToRange(_percentLoaded); 
        }

        public function set percentLoaded(value:Number) : void {
            if (isNaN(value) || !isFinite(value)) value = 0;		 
            _percentLoaded = value; 
        }
        /** The ratio (0-1) loaded (number of items loaded / number of items total) */
        public function get ratioLoaded() : Number { 
            return truncateToRange(_ratioLoaded); 
        }

        public function set ratioLoaded(value:Number) : void { 
            if (isNaN(value) || !isFinite(value)) value = 0;		
            _ratioLoaded = value; 
        }

        public function truncateToRange(value:Number):Number{
            if(value < 0){
                value = 0;
            }else if (value > 1){
                value =  1
            }else if (isNaN(value) || !isFinite(value)){
                value = 0;
            }
            return value;
        }

        override public function toString() : String{
            return super.toString();
        }

    }

}
