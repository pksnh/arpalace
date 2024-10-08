using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GPS : MonoBehaviour
{
    public Text textMsg;

    int Age;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        if(!Input.location.isEnabledByUser)
        {
            yield break; // �ڷ�ƾ �Լ� �ݺ� Ż�� �� ����(yield return;-���� ������ �� �ٽ� ���ƿ�)
        }

        Input.location.Start(); // ��ġ�� �޾ƿ����� ��, but �ٷ� �޾ƿ��� ���� �ƴ�

        int maxWait = 30;

        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0) // ���� �ʱ�ȭ �����̰� maxWait�� 0���� ũ�ٸ�
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        if(maxWait < 1)
        {
            textMsg.text = "Time Out";
        }

        if (Input.location.status == LocationServiceStatus.Failed)
        {
            textMsg.text = "Location is not Detected";
            yield break;
        }
        else
        {
            while(true)
            {
                textMsg.text = "��ġ: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.horizontalAccuracy; // ����(x�� ����)+�浵(y�� ����)+���� ��Ȯ��(��ġ�� ��Ȯ�Ǽ� �ݰ��� ���� ������ ��Ÿ��, ������ ��������, �� ���� ����� �̵��ϸ� ��ġ ���� ����)
                yield return new WaitForSeconds(1);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector2 GetGPSInformation()
    {
        Vector2 vectorPosition = new Vector2(Input.location.lastData.latitude, Input.location.lastData.longitude);

        return vectorPosition;
    }
}
