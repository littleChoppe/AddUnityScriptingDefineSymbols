using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entry : MonoBehaviour {

	//程序入口
	void Start ()
    {
#if XIAO_LU
        Debug.Log("小鹿宏定义");
#else
        Debug.Log("没有使用宏定义");
#endif
    }
	
}
