using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDelegate : MonoBehaviour {
    // // // --------------- test delegate
    // public delegate string MyDelegate (string value);
    // public MyDelegate delegateMethod;
    // public class MyClass {
    //     public string a;
    //     public string b;
    //     public MyClass () {
    //         Debug.Log ("new MyClass");
    //     }
    //     public MyClass (string value) {
    //         a = value;
    //         Debug.Log ("new MyClass: " + a);
    //     }
    //     public string show (string msg) {
    //         string showMsg = "show: " + msg + a;
    //         Debug.Log (showMsg);
    //         return showMsg;
    //     }
    // }

    // private void test () {
    //     Type t = Type.GetType ("Project.MyClass");
    //     ConstructorInfo ci = t.GetConstructor (Type.EmptyTypes);
    //     if (ci != null) {
    //         MyClass obj = (MyClass) ci.Invoke (new object[] { });
    //         obj.show ("aa");
    //     }

    //     object dObj = Activator.CreateInstance (t, new object[] { "sss" });
    //     MethodInfo method = t.GetMethod ("show");
    //     BindingFlags flag = BindingFlags.Public | BindingFlags.Instance;
    //     object showMsg = method.Invoke (dObj, flag, Type.DefaultBinder, new object[] { "msg" }, null);
    // }

    // private void test1 () {
    //     Type t = Type.GetType ("GamePlay+MyClass");
    //     object obj = Activator.CreateInstance (t, new object[] { });
    //     MethodInfo method = t.GetMethod ("show", BindingFlags.Public);
    //     // MyDelegate method1 = delegateMethod (t.GetMethod ("show"));
    //     String returnValue = method.Invoke (obj, new object[] { "aa" });
    // }
    // private void Start () {
    //     test1 ();
    // }
    // --------------- test delegate
}