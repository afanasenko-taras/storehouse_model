using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkladModel;
using System.Linq;
using ExtendedXmlSerializer.Configuration;
using ExtendedXmlSerializer;
using System.IO;
using System.Xml;
using System;
using Object = UnityEngine.Object;
using Unity.VisualScripting;
using ControlModel;
using TMPro;
using UnityEngine.UI;

public class main : MonoBehaviour
{

    public static Vector2 getPosition(double x, double y)
    {
        return new Vector2((float)x - 10, -(float)y+8);
    }

    public Transform[] brick;
    public Transform AntBotTransform;
    public Text timerTMP;
    Dictionary<string, AntBotUnity> antsBot = new Dictionary<string, AntBotUnity>();
    DateTime startTime;
    AntStateChange asc;
    Dictionary<string, (int, int)> lastPosition = new Dictionary<string, (int, int)>();
    List<AntStateChange> logs = new List<AntStateChange>();
    public Transform panel;
    TimeSpan time_shift = TimeSpan.FromMinutes(0);

    // Start is called before the first frame update
    void Start()
    {
        string filePath = @"./field_b.csv";
        List<string[]> listA = new List<string[]>();
        StreamReader reader = null;
        if (File.Exists(filePath))
        {
            reader = new StreamReader(File.OpenRead(filePath));
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(';');
                listA.Add(values);
            }
        }
        else
        {
            Debug.Log("File doesn't exist");
        }


        int uid = 0;
        int y = 0;
        foreach (var line in listA)
        {
            int x = 0;
            foreach (var v in line) {
                Debug.Log(x + "," + y);
                int t = 0;
                if (v == "b")
                    t = 0;
                if (v == "g")
                    t = 1;
                if (v == "i")
                    t = 3;
                if (v == "o")
                    t = 2;
                if (v == "r") {
                    t = 1;
                    Transform ab = Instantiate(AntBotTransform);
                    ab.SetParent(panel);
                    AntStateChange asc = new AntStateChange();
                    asc.uid = uid.ToString();
                    asc.xCoordinate = x;
                    asc.yCoordinate = y;
                    lastPosition.Add(asc.uid, (x, y));
                    asc.lastUpdated = 0;
                    uid++;
                    antsBot.Add(asc.uid, ab.GetComponent<AntBotUnity>());
                    antsBot[asc.uid].antStateChange = asc;
                    antsBot[asc.uid].SetPosition();
                    antsBot[asc.uid].startTime = startTime; 
                }


                Transform br = Object.Instantiate(brick[t]);
                br.SetParent(panel);
                br.SetPositionAndRotation(getPosition(x, y), Quaternion.identity);
                x++;
            }
            y++;
        }


        filePath = @"./model_1.txt";
        reader = null;
        if (File.Exists(filePath))
        {
            reader = new StreamReader(File.OpenRead(filePath));
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(' ');
                int event_id = int.Parse(values[0]);
                int time_event = int.Parse(values[1]);
                string ant_id = values[2];
                string event_type = values[3];
                int xx = int.Parse(values[4])+1;
                int yy = int.Parse(values[5])+1;
                AntStateChange abc = new AntStateChange();
                abc.uid = ant_id;
                if ((event_type == "W") || (event_type == "RCW") || (event_type == "RCCW") || (event_type == "L") || (event_type == "UL"))
                {
                    abc.xCoordinate = xx;
                    abc.yCoordinate = yy;
                    abc.xSpeed = 0;
                    abc.ySpeed = 0;
                    abc.lastUpdated = time_event;
                    lastPosition[abc.uid] = (xx, yy);
                    logs.Add(abc);
                }
                else if ((event_type == "MF") || (event_type == "MB"))
                {
                    int xl = int.Parse(values[4])+1;
                    int yl = int.Parse(values[5])+1;
                    xx = int.Parse(values[6])+1;
                    yy = int.Parse(values[7])+1;
                    abc.lastUpdated = time_event;
                    abc.xCoordinate = xl;
                    abc.yCoordinate = yl;
                    abc.xSpeed = (xx - xl);
                    abc.ySpeed = (yy - yl);
                    logs.Add(abc);
                }
                if (event_type == "L")
                {
                    abc.command = "Load";
                }
                if (event_type == "UL")
                {
                    abc.command = "Unload";
                }
            }
        }
        else
        {
            Debug.Log("File doesn't exist");
        }



        startTime = DateTime.Now;
        foreach(var ant in antsBot.Values)
        {
            ant.startTime = startTime;
        }


        asc = logs[0];
    }

    int count = 0;
    void Update()
    {
        TimeSpan current = DateTime.Now - startTime;
        timerTMP.text = string.Format("{0:dd\\.hh\\:mm\\:ss}", current);
        Color startColor = new Color((float)(192.0 / 255.0), (float)(101.0 / 255.0), (float)(101.0 / 255.0));
        if (current.TotalSeconds > asc.lastUpdated) 
        {
            do
            {
                if (asc.command == "Create AntBot")
                {

                    //StartCoroutine(antsBot[asc.uid].ChangeCollor(0, Color.red));
                }
                else
                {
                    antsBot[asc.uid].antStateChange = asc;
                    antsBot[asc.uid].SetPosition();
                }

                if (asc.lastUpdated > time_shift.TotalSeconds) {
                    if (asc.command == "Rotate")
                    {
                        if (asc.isXDirection)
                        {
                            transform.rotation = Quaternion.Euler(0, 0, 90);
                        }
                        else
                        {
                            transform.rotation = Quaternion.Euler(0, 0, 0);
                        }
                        StartCoroutine(antsBot[asc.uid].RotateMe(new Vector3(0, 0, 90), 4));
                    }
                    if (asc.command == "Load")
                    {
                        StartCoroutine(antsBot[asc.uid].ChangeCollor(1, Color.yellow));
                    }
                    if (asc.command == "Unload")
                    {
                        //Color32 color = new Color32((byte)(255 * antsBot[asc.uid].antStateChange.charge / 7200), 0, 0, 255);
                        StartCoroutine(antsBot[asc.uid].ChangeCollor(1, startColor));
                    }
                }

                count++;
                asc = logs[count];
            } while (asc.lastUpdated < current.TotalSeconds);
        }
    }
}
