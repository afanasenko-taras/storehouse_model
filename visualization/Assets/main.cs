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

public class main : MonoBehaviour
{

    public static Vector2 getPosition(double x, double y)
    {
        return new Vector2((float)x - 10, -(float)y+8);
    }

    public Transform[] brick;
    public Transform AntBotTransform;
    public TextMeshProUGUI timerTMP;
    Dictionary<string, AntBotUnity> antsBot = new Dictionary<string, AntBotUnity>();
    private SkladWrapper skladWrapper;
    Sklad sklad;
    DateTime startTime;
    AntStateChange asc;
    public Transform panel;
    AntStateChange[] logs;
    TimeSpan time_shift = TimeSpan.FromMinutes(201);

    // Start is called before the first frame update
    void Start()
    {
        System.Random rnd = new System.Random(DateTime.Now.Millisecond);
        SkladWrapper skladWrapper = new SkladWrapper(@"wms-config.xml", false);
        skladWrapper.AddLogger();
        skladWrapper.AddSklad();
        skladWrapper.AddAnts();
        new MoveSort(skladWrapper).Run(TimeSpan.FromSeconds(0));

         //SkladLogger logger = (SkladLogger)skladWrapper.objects.First(x => x is SkladLogger);
        //logs = logger.logs.ToArray();
        logs = SkladWrapper.DeserializeXML<AntStateChange[]>(File.ReadAllBytes("log_unity.xml"));


        sklad = (Sklad) skladWrapper.objects.First(x=>x is Sklad);

        foreach (var yl in sklad.skladLayout)
        {
            int y = yl.Key;
            foreach (var xl in yl.Value) {
                Transform br = Object.Instantiate(brick[xl.Value]);
                br.SetParent(panel);
                int x = xl.Key;
                br.SetPositionAndRotation(getPosition(x, y), Quaternion.identity);
            }
        }
        startTime = DateTime.Now - time_shift;
        asc = logs[0];
    }

    int count = 0;
    void Update()
    {
        TimeSpan current = DateTime.Now - startTime;
        timerTMP.text = string.Format("{0:dd\\.hh\\:mm\\:ss}", current);
        if (current.TotalSeconds > asc.lastUpdated) 
        {
            do
            {
                if (asc.command == "Create AntBot")
                {
                    Transform ab = Instantiate(AntBotTransform);
                    ab.SetParent(panel);
                    antsBot.Add(asc.uid, ab.GetComponent<AntBotUnity>());
                    antsBot[asc.uid].antStateChange = asc;
                    antsBot[asc.uid].SetPosition();
                    antsBot[asc.uid].startTime = startTime;
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
                        StartCoroutine(antsBot[asc.uid].ChangeCollor(2, Color.green));
                    }
                    if (asc.command == "Unload")
                    {
                        Color32 color = new Color32((byte)(255 * antsBot[asc.uid].antStateChange.charge / 7200), 0, 0, 255);
                        StartCoroutine(antsBot[asc.uid].ChangeCollor(1, color));
                    }
                }



                count++;
                asc = logs[count];
            } while (asc.lastUpdated < current.TotalSeconds);
        }
    }
}
