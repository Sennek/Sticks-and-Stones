using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class write {

     //specify streamwriter
    

    //private void Start()
    //{
        
    //}
     
    public void writeToTxt()
    {
        StreamWriter sw;
        sw = new StreamWriter("Assets/Resources/test.txt", true); //use streamwriter to write to
        sw.WriteLine("this is test");
        sw.Close();
    }

    //private void Update()
    //{
    //    sw.WriteLine("this is test");

    //    sw.Flush();
    //}

}
