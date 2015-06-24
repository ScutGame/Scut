package ScutNetwork
{
	public class SingleLinkList
	{
		
		public var firstNode:SingleLinkNode;
		
		public function SingleLinkList()
		{
			
		}
		
		/** 
		 * 链表是否为空 
		 * @return 
		 */  
		public function isEmpty():Boolean
		{
			return firstNode==null;
		}
		
		/** 
		 * 此节点是否是最后一个节点 
		 * @param node 要判断的节点 
		 * @return 是否是最后一个节点 
		 */
		public function isEnd(node:SingleLinkNode):Boolean
		{
			return node==null;
		}
		
		/**
		 * 显示链表所有节点信息
		 *
		 */
		public function toString():void{
			var pNode:SingleLinkNode = firstNode;
			while(pNode!=null) {
				trace(" - - "+pNode.data);
				pNode = pNode.next;
			}
			pNode = null;
		}  
		
		
		/** 
		 * 在链表的最前面插入一个值
		 * @param data 要插的数据
		 * @return 是否成功
		 */   
		public function insertFirst(data:String):Boolean{
			var node:SingleLinkNode= new SingleLinkNode(data);
			if(isEmpty()) {
				firstNode = node;
				return true;
			}
			//将新插入的数的地址指向原来的第一位数
			node.next = firstNode;
			//更新插入值后的新链表的头  
			firstNode = node;
			node = null;
			return true;
		}
		
		
		/** 
		 * 在链表的最末插入一个值 
		 * @param data 要插入的数据 
		 * @return 是否成功 
		 */  
		public function insertLast(data:String):Boolean
		{
			var node:SingleLinkNode = new SingleLinkNode(data);
			if(isEmpty()) {
				//node = head;
				firstNode = node;
				return true;
			}
			var p:SingleLinkNode = firstNode;
			var pre:SingleLinkNode = firstNode;
			//遍历整个链表,从而最终得到一个最后的节点  
			while(!isEnd(p)) {
				// 在向后移动之前得到一个节点  
				pre = p;
				// 逐次向后移动  
				p = p.next;
			}
			// 将要插入的值插入到最后一个节点上  
			pre.next = node;
			node = null;
			return false;
		}
		
		
		/** 
		 * 在某节点前插入一新数据 
		 * @param oldData 原节点 
		 * @param newData 新节点 
		 * @return 是否插入成功 
		 */  
		public function insertBefore(oldData:String,newData:String):Boolean{
			var preNode:SingleLinkNode = find(oldData,true);
			
			if(preNode==null) {
				return false;
			}
			var newNode:SingleLinkNode = new SingleLinkNode(newData);
			if(preNode==firstNode) {
				newNode.next=firstNode;
				firstNode = newNode;
			}else {
				var pNode:SingleLinkNode = preNode.next;
				newNode.next=pNode;
				preNode.next=newNode;
			}
			preNode = null;
			return true;
		}
		
		/** 
		 * 在某节点后插入一新数据 
		 * @param oldData 原节点 
		 * @param newData 新节点 
		 * @return 是否插入成功 
		 */  
		public function insertAfter(oldData:String,newData:String):Boolean{
			var preNode:SingleLinkNode = find(oldData,false);
			
			if(preNode==null) {
				return false;
			}
			//if(preNode==head) {
			
			trace(" insert ");
			var newNode:SingleLinkNode = new SingleLinkNode(newData);
			var pNode:SingleLinkNode = preNode.next;
			newNode.next = pNode;
			preNode.next = newNode;//pNode;
			newNode = null;
			//}
			preNode = null;
			return true;
		}
		
		/** 
		 * 删除某一节点 
		 * @param data 节点数据 
		 * @return 是否删除成功 
		 */  
		public function remove(data:String):Boolean{
			if(isEmpty()) {
				return false;
			}
			var preNode:SingleLinkNode = find(data, true);
			if(preNode == firstNode) {
				firstNode = firstNode.next;
			}else {
				var pNode:SingleLinkNode = preNode.next;
				preNode.next=pNode.next;
			}
			preNode = null;
			return true;
		}
		
		/** 
		 * 将某节点数据更新为新的数据 
		 * @param oldData 
		 * @param newData 
		 * @return 
		 */  
		public function update(oldData:String,newData:String):Boolean{
			var pNode:SingleLinkNode = find(oldData, false);
			if(pNode!=null) {
				pNode.data = newData;
				return true;
			}
			return false;
		}
		
		
		/** 
		 * 查找数值为data的节点 
		 * @param flag 为false时表示返回要找数据的节点, 
		 *             为true时表示返回要找数据之前的节点 
		 * @param data 要查找的数值 
		 * @return  
		 */  
		public function find(data:String,flag:Boolean):SingleLinkNode
		{
			trace(data+"  ---  "+flag);
			var p:SingleLinkNode = firstNode;
			var pre:SingleLinkNode = firstNode;
			while(!isEnd(p) && p.data != data) {
				// 保存之前的信息  
				pre = p;
				//逐次向后移动  
				p = p.next;
			}
			if(isEnd(p)) {
				return null;  
			}
			if(flag) return pre;
			else return p;
		}
		
		
		/**
		 * 返回链表个数 
		 * @return 
		 * 
		 */		
		public function getCount():uint
		{
			var length:int=0;
			var node:SingleLinkNode=this.firstNode;
			while(node!=null)
			{
				length++;
				node=node.next;
			}
			return length;
		}
		
	}
}