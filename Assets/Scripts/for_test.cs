using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class for_test : MonoBehaviour
{
    float[][] testArray = new float[3][];

    void Start()
    {
       /* MyClass mc = new MyClass();
        Debug.Log("The value of property Count: " + mc.Count);
        mc.Count = 20;
        Debug.Log("The value of changed Count: " + mc.Count);*/

        //Checking microphone device names
        foreach(var device in Microphone.devices){
            Debug.Log("Name: " + device);
        }
    }


    public class MyClass{
        private int count = 10;// Field

        public int Count{// Property
            get{
                return count;// To reference the field value
            }

            set{
                this.count = value;// To set the field value
            }
        }
    }
}
