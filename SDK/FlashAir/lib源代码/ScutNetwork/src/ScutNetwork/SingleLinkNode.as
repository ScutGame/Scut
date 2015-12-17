package ScutNetwork
{
	
	/**
	 * 单链表节点类 
	 * @author xiaor
	 * 
	 */	
	public class SingleLinkNode
	{
		public var data:String;
		public var next:SingleLinkNode=null;
		
		public function SingleLinkNode(_data:String)
		{
			this.data=_data;
		}
	}
}