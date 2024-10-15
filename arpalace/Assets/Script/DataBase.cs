using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using UnityEditor;
using static DataBase;

public class DataBase : MonoBehaviour
{
    public TextMeshProUGUI[] buildingText;
    public TextMeshProUGUI buildingNameText;
    public TextMeshProUGUI buildingActText;
    public TextMeshProUGUI buildingInformationText;

    int i = 0;

    public class Palace
    {
        public string name;
        public string act;
        public string information;

        public Palace() // MonoBehaviour 상속 받은 클래스는 생성자 작성 필요 없음(MonoBehaviour에 생성자 존재), 아닌 클래스는 생성자 써야 함
        {

        }

        public Palace(string name, string act, string information) // 파라미터 입력하는 생성자 만들 경우 디폴트 생성자도 기본적으로 만들어야 함
        {
            this.name = name;
            this.act = act;
            this.information = information;
        }
    }

    DatabaseReference reference;

    private void Awake()
    {
        reference = FirebaseDatabase.DefaultInstance.RootReference; // 데이터베이스 정보가 있는 줄의 바로 아래 줄 참조(정보가 하나도 없으면 맨 위 참조)

        writeNewBuilding("palace_00", "돈화문", "창덕궁 정문", "·동궐도 제작 당시(1828~1830년)에는 팔작지붕 사용\n·현재는 우진각지붕 사용");
        writeNewBuilding("palace_01", "인정전", "창덕궁 정전", "·1777년(정조 1)에 관료 질서 재정비 위해 품계석 설치\n·1907년 전후로 내부에 전등, 커튼 등 서양 실내장식 설치");
        writeNewBuilding("palace_02", "희정당", "창덕궁 편전", "·1917년 화재로 소실, 20년에 경복궁 강년전을 옮겨와 증축\n·남면에 자동차 진입용 지붕 존재, 내부는 서양식으로 장식");
        writeNewBuilding("palace_03", "대조전", "창덕궁 중궁전", "·1917년 화재로 소실, 20년에 경복궁 교태전 옮겨와 재건\n·재건 당시 내부를 서양식으로 장식해 원형과 많이 다름");
        writeNewBuilding("palace_04", "낙선재", "주거용 건물", "·1847년(현종 13)에 건립되어 동궐도에는 없음\n·사대부 주택 형식, 황실 일가가 삶의 마지막을 보낸 장소");
        writeNewBuilding("palace_05", "주합루(2층, 1층: 규장각)", "서고 및 학술·정책 연구 기관", "·1781년(정조 5년) 규장각을 학문·정책 연구 기관으로 확장\n·1층 규장각은 왕실 서고, 2층 주합루는 정사 논쟁·독서 장소");
        writeNewBuilding("palace_06", "부용지", "주합루의 연못", "·정조가 부용지에서 신하들과 함께 행사 참여 및 낚시\n·작시 대회에서 미제출자는 부용지 내 섬으로 유배");
        writeNewBuilding("palace_07", "연경당", "주거용 건물", "·효명세자가 순조와 순원왕후를 위한 잔치를 위해 지음\n·동궐도에 'ㄷ'자 형태 전각으로 묘사, 현재는 사대부 집 형식");
        writeNewBuilding("palace_08", "선정전", "창덕궁 편전", "·현재 유일하게 보존된 청기와 건물\n·후기에는 주로 의례적·제례적 장소로 사용");
    }

    // Start is called before the first frame update
    void Start()
    {
        SendDbToText();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void writeNewBuilding(string buildingIndex, string buildName, string act, string information)
    {
        Palace palace = new Palace(buildName, act, information);
        string str = JsonUtility.ToJson(palace);

        reference.Child("buildings").Child(buildingIndex).SetRawJsonValueAsync(str);
    }


    /*
    public void CreateBulidingPosition(Vector2 gpsPos, Transform image)
    {
        FirebaseDatabase.DefaultInstance.GetReference("Stores").GetValueAsync().ContinueWithOnMainThread(task =>  // ContinueWithOnmainThread: 데이터베이스에서 정보 계속 불러옴
        {
            // 비동기화: Async->각자가 서로에게 영향 안 미치고 각자 작동함 
            if (task.IsFaulted) // 정보 받아오기 실패함
            {


            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result; // DataSnapshot: 데이터를 가져왔을 때 그 찰나에 받아왔던 값
                foreach (DataSnapshot data in snapshot.Children) // 스냅샷.children: 정보 2개 있음, 정보들을 DataSnapShot 타입 data로 정보 개수만큼 반복해서 정보 보냄
                {
                    string value = data.GetRawJsonValue(); // DegualtInstance->SnapShot->Foreach 반복->GetRawjsonValue로 개별 정보 전달
                    Palace palace = JsonUtility.FromJson<Palace>(value); // string 타입으로 Json문법으로 작성된 value를 Store 타입으로 변경, JsonUtility: 유니티에서 Json타입 정보 다룰 수 있게 함, Json: 네트워크로 정보 주고받을 시 string 타입으로 다룸

                    
                    //bVector2 dbPos = new Vector2(palace.act, palace.information);
                    //float d = Vector2.Distance(dbPos, gpsPos); // dPos와 gpsPos 거리 계산
                    //if (d < 0.0005f)
                    //{
                    //    GameObject prefab = Resources.Load(palace.name) as GameObject;
                    //    Instantiate(prefab, image.position, image.rotation); // Resource: 특별한 폴터, .Load 호출 시에만 파일 로드, 일반적으로는 호출x
                    //}
                    
                }
            }
        }
        );
    }
    */

    public void SendDbToText()
    {
        FirebaseDatabase.DefaultInstance.GetReference("buildings").GetValueAsync().ContinueWithOnMainThread(task =>  // ContinueWithOnmainThread: 데이터베이스에서 정보 계속 불러옴
        {
            // 비동기화: Async->각자가 서로에게 영향 안 미치고 각자 작동함 
            if (task.IsFaulted) // 정보 받아오기 실패함
            {

            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result; // DataSnapshot: 데이터를 가져왔을 때 그 찰나에 받아왔던 값
                foreach (DataSnapshot data in snapshot.Children) // 스냅샷.children: 정보 2개 있음, 정보들을 DataSnapShot 타입 data로 정보 개수만큼 반복해서 정보 보냄
                {
                    string value = data.GetRawJsonValue();
                    Palace palace = JsonUtility.FromJson<Palace>(value);

                    buildingText[i].text = "이름: " + palace.name + "\n\n기능: " + palace.act + "\n\n정보\n" + palace.information;
                    i++;
                }
            }
        });
    }
}