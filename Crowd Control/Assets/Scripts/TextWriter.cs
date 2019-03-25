﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class TextWriter : MonoBehaviour
{
    public void AppendWrite<T>(string file, T data){
        StreamWriter sw = new StreamWriter(file,true);
        string toadd = Time.time + "," + data;
        sw.WriteLine(toadd);
        sw.Close();
    }
}