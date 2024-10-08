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

        public Palace() // MonoBehaviour ��� ���� Ŭ������ ������ �ۼ� �ʿ� ����(MonoBehaviour�� ������ ����), �ƴ� Ŭ������ ������ ��� ��
        {

        }

        public Palace(string name, string act, string information) // �Ķ���� �Է��ϴ� ������ ���� ��� ����Ʈ �����ڵ� �⺻������ ������ ��
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
        reference = FirebaseDatabase.DefaultInstance.RootReference; // �����ͺ��̽� ������ �ִ� ���� �ٷ� �Ʒ� �� ����(������ �ϳ��� ������ �� �� ����)

        writeNewBuilding("00", "��ȭ��", "â���� ����", "1609��(���ر� 1��)�� ����� �� ���ݱ��� ����");
        writeNewBuilding("01", "������", "â���� ����", "1609��(���ر� 1��)�� ���, 1777��(���� 1��)�� ���� ���� ������ ���� ǰ�輮 ��ġ");
        writeNewBuilding("02", "������", "â���� ����", "�Ƿ� ��ҷ� �ַ� ���Ǵ� �������� ����� �������� ���, 1917�� ȭ��� �ҽǵ� �� 1920�⿡ �溹�� �������� �Űܿ� ����,  �ǹ� ���鿡 �ڵ��� ���� ���� ���� ����, ���δ� ��������� ���");
        writeNewBuilding("03", "������", "â���� ����", "1917�� ȭ��� �ҽ� �� 1920�⿡ �溹�� ������ �Űܿ� ���, ���θ� ��������� ���");
        writeNewBuilding("04", "������", "â���� ħ��", "1847��(���� 13��)�� �Ǹ�, �ñ�ħ�������� Ȱ���� ���� ���� ���� ����, �������� Ȳ�� �ϰ��� ���� �������� ���� ���");
        writeNewBuilding("05", "���尢", "���� �� �м�, ��å ���� ���", "1781��(���� 5��) �ս� ���� ���� ���� ���尢�� �й� �� ��å ���� ������� Ȯ��, ��ȭ���� ���� �� ����, ����� �Ǳ� ���� ���� �ܱ��尢 ��ġ");
        writeNewBuilding("06", "�ο���", "", "");
        writeNewBuilding("07", "�����", "", "");
        writeNewBuilding("08", "������", "â���� ����", "û��ȭ �ǹ� �� �����ϰ� �����ϴ� �ǹ�");
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
        FirebaseDatabase.DefaultInstance.GetReference("Stores").GetValueAsync().ContinueWithOnMainThread(task =>  // ContinueWithOnmainThread: �����ͺ��̽����� ���� ��� �ҷ���
        {
            // �񵿱�ȭ: Async->���ڰ� ���ο��� ���� �� ��ġ�� ���� �۵��� 
            if (task.IsFaulted) // ���� �޾ƿ��� ������
            {

            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result; // DataSnapshot: �����͸� �������� �� �� ������ �޾ƿԴ� ��
                foreach (DataSnapshot data in snapshot.Children) // ������.children: ���� 2�� ����, �������� DataSnapShot Ÿ�� data�� ���� ������ŭ �ݺ��ؼ� ���� ����
                {
                    string value = data.GetRawJsonValue(); // DegualtInstance->SnapShot->Foreach �ݺ�->GetRawjsonValue�� ���� ���� ����
                    Palace palace = JsonUtility.FromJson<Palace>(value); // string Ÿ������ Json�������� �ۼ��� value�� Store Ÿ������ ����, JsonUtility: ����Ƽ���� JsonŸ�� ���� �ٷ� �� �ְ� ��, Json: ��Ʈ��ũ�� ���� �ְ���� �� string Ÿ������ �ٷ�

                    /*
                    bVector2 dbPos = new Vector2(palace.act, palace.information);
                    float d = Vector2.Distance(dbPos, gpsPos); // dPos�� gpsPos �Ÿ� ���
                    if (d < 0.0005f)
                    {
                        GameObject prefab = Resources.Load(palace.name) as GameObject;
                        Instantiate(prefab, image.position, image.rotation); // Resource: Ư���� ����, .Load ȣ�� �ÿ��� ���� �ε�, �Ϲ������δ� ȣ��x
                    }
                    */
                }
            }
        }
        );
    }

}
