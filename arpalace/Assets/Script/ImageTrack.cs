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
        string name = t.referenceImage.name; // 추적한 이미지의 이름을 string 타입 name에 입력

        // GameObject o = dict1[name]; /* dict[키값]=벨류값 return */

        if (dict1.TryGetValue(name, out GameObject o)) // 만약 dict1에 name와 같은 키값이 있어 o에 벨류값 넣어 반환된다면
        {
            if (t.trackingState == UnityEngine.XR.ARSubsystems.TrackingState.Tracking) // t의 이미지 추적 상태가 추적 중이라면(즉, 이미지가 보인다면)
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

    void OnDisable() // 프로그램이 비활성화할 때 사용, delete, 마무리 개념
    {
        manager.trackedImagesChanged -= OnChanged; // -=: 연결된 함수 연결 해제
    }

    // Update is called once per frame
    void Update()
    {

    }
}
