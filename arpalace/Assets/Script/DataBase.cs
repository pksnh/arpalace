using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using Firebase;
using Firebase.Database;
using Firebase.Extensions;

public class DataBase : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;


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

    // Start is called before the first frame update
    void Start()
    {
        reference = FirebaseDatabase.DefaultInstance.RootReference; // 데이터베이스 정보가 있는 줄의 바로 아래 줄 참조(정보가 하나도 없으면 맨 위 참조)

        writeNewBuilding("00", "돈화문", "창덕궁 정문", "1609년(광해군 1년)에 재건한 후 지금까지 보전");
        writeNewBuilding("01", "인정전", "창덕궁 정전", "1609년(광해군 1년)에 재건, 1777년(정조 1년)에 관료 질서 재정비 위해 품계석 설치");
        writeNewBuilding("02", "희정당", "창덕궁 편전", "의례 장소로 주로 사용되는 선정전을 대신해 편전으로 기능, 1917년 화재로 소실된 후 1920년에 경복궁 강년전을 옮겨와 증축,  건물 남면에 자동차 진입 위한 지붕 존재, 내부는 서양식으로 장식");
        writeNewBuilding("03", "대조전", "창덕궁 내전", "1917년 화재로 소실 후 1920년에 경복궁 교태전 옮겨와 재건, 내부를 서양식으로 장식");
        writeNewBuilding("04", "낙선재", "창덕궁 침전", "1847년(현종 13년)에 건립, 궁궐침전형식을 활용한 사대부 주택 형식 지님, 대한제국 황실 일가가 삶의 마지막을 보낸 장소");
        writeNewBuilding("05", "규장각", "서고 및 학술, 정책 연구 기관", "1781년(정조 5년) 왕실 서고 역할 맡은 규장각을 학문 및 정책 연구 기관으로 확장, 강화도에 어제 및 어필, 어람용 의궤 보관 위한 외규장각 배치");
        writeNewBuilding("06", "부용지", "", "");
        writeNewBuilding("07", "연경당", "", "");
        writeNewBuilding("08", "선정전", "창덕궁 편전", "청기화 건물 중 유일하게 현존하는 건물");
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

                    /*
                    bVector2 dbPos = new Vector2(palace.act, palace.information);
                    float d = Vector2.Distance(dbPos, gpsPos); // dPos와 gpsPos 거리 계산
                    if (d < 0.0005f)
                    {
                        GameObject prefab = Resources.Load(palace.name) as GameObject;
                        Instantiate(prefab, image.position, image.rotation); // Resource: 특별한 폴터, .Load 호출 시에만 파일 로드, 일반적으로는 호출x
                    }
                    */
                }
            }
        }
        );
    }

}
