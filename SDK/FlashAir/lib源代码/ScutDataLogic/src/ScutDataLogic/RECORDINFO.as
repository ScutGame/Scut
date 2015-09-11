package ScutDataLogic
{
	public class RECORDINFO
	{
		public var nRecordSize:int;
		public var nRecordReadSize:int;
		
		public function RECORDINFO(_RecordSize:int, _RecordReadSize:int)
		{
			nRecordSize		= _RecordSize;
			nRecordReadSize	= _RecordReadSize; 
		}
	}
}