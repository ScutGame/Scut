using UnityEngine;
using System.Collections;

public interface IHttpCallback {
	
	void OnHttpRespond(HttpPackage package, object ud);
	
}
