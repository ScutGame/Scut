package ScutDataLogic
{
	public class CInt64
	{
		private var mValue:Number;
		
		public function CInt64(value:* = 0)
		{
			if(value<0)
			{
				var error:String="";
			}
			
			if(value is Number)
			{
				mValue = Math.floor(value);
			}
			else if(value is CInt64)
			{
				mValue = value.getValue();
			}
			else if(value is String)
			{
				mValue = Number(value);
			}
		}
		
		public function getValue():Number
		{
			return mValue;
		}
		
		public function str() : String
		{
			return mValue.toString();
		}
		
		public function op_Equality(value:CInt64):Boolean
		{
			return mValue == value.mValue;
		}
		
		public function equal(value:CInt64):Boolean
		{
			return mValue == value.mValue;
		}
		
		public function op_LessThan(value:CInt64):Boolean
		{
			return mValue < value.mValue;
		}
		
		public function op_LessThanOrEqual(value:CInt64):Boolean
		{
			return mValue <= value.mValue;
		}
		
		public function op_Addition(value:*):CInt64
		{
			if(value is Number)
			{
				return new CInt64(mValue + value);
			}else if(value is CInt64)
			{
				return new CInt64(mValue + value.mValue);
			}
			
			return null;
		}
		
		public function op_Subtraction(value:*):CInt64
		{
			if(value is Number)
			{
				return new CInt64(mValue - value);
			}else if(value is CInt64)
			{
				return new CInt64(mValue - value.mValue);
			}
			
			return null;
		}
		
		public function op_Multiply(value:*):CInt64
		{
			if(value is Number)
			{
				return new CInt64(mValue * value);
			}else if(value is CInt64)
			{
				return new CInt64(mValue * value.mValue);
			}
			
			return null;
		}
		
		public function op_Division(value:*):CInt64
		{
			if(value is Number)
			{
				return new CInt64(mValue / value);
			}else if(value is CInt64)
			{
				return new CInt64(mValue / value.mValue);
			}
			
			return null;
		}
		
		public function op_Modulus(value:*):CInt64
		{
			if(value is Number)
			{
				return new CInt64(mValue % value);
			}else if(value is CInt64)
			{
				return new CInt64(mValue % value.mValue);
			}
			
			return null;
		}
	}
}