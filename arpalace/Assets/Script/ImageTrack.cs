using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;

public class ImageTrack : MonoBehaviour
{
    public ARTrackedImageManager manager;
    public List<GameObject> list1 = new List<GameObject>();
    //public List<AudioClip> list2 = new List<AudioClip>();

    private Dictionary<string, GameObject> dict1 = new Dictionary<string, GameObject>();
    //private Dictionary<string, AudioClip> dict2 = new Dictionary<string, AudioClip>();

    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject o in list1)
        {
            dict1.Add(o.name, o);
        }

        /*foreach (AudioClip o in list2)
        {
            dict2.Add(o.name, o);
        }*/
    }

    /*void UpdateSound(ARTrackedImage t)
    {
        string name = t.referenceImage.name;

        AudioClip sound = dict2[name];
        GetComponent<AudioSource>().PlayOneShot(sound);
    }*/

    private void UpdateImage(ARTrackedImage t)
    {
        string name = t.referenceImage.name; // ������ �̹����� �̸��� string Ÿ�� name�� �Է�

        // GameObject o = dict1[name]; /* dict[Ű��]=������ return */

        if (dict1.TryGetValue(name, out GameObject o)) // ���� dict1�� name�� ���� Ű���� �־� o�� ������ �־� ��ȯ�ȴٸ�
        {
            if (t.trackingState == UnityEngine.XR.ARSubsystems.TrackingState.Tracking) // t�� �̹��� ���� ���°� ���� ���̶��(��, �̹����� ���δٸ�)
            {
                o.transform.position = t.transform.position;
                o.transform.rotation = t.transform.rotation;

                o.SetActive(true);
            }
            else
            {
                o.SetActive(false);
            }
        }
    }

    private void OnChanged(ARTrackedImagesChangedEventArgs args)
    {
        foreach (ARTrackedImage t in args.added)
        {
            UpdateImage(t);
            //UpdateSound(t);
        }

        foreach (ARTrackedImage t in args.updated)
        {
            UpdateImage(t);
        }
    }

    private void OnEnable()
    {
        manager.trackedImagesChanged += OnChanged;
    }

    void OnDisable() // ���α׷��� ��Ȱ��ȭ�� �� ���, delete, ������ ����
    {
        manager.trackedImagesChanged -= OnChanged; // -=: ����� �Լ� ���� ����
    }

    // Update is called once per frame
    void Update()
    {

    }
}
