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
            yield break; // 코루틴 함수 반복 탈출 및 중지(yield return;-다음 프레임 때 다시 돌아옴)
        }

        Input.location.Start(); // 위치값 받아오도록 함, but 바로 받아오는 것은 아님

        int maxWait = 30;

        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0) // 만약 초기화 상태이고 maxWait가 0보다 크다면
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
                textMsg.text = "위치: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.horizontalAccuracy; // 위도(x축 기준)+경도(y축 기준)+수평 정확도(위치의 불확실성 반경을 미터 단위로 나타냄, 일종의 오차범위, 이 범위 벗어나서 이동하면 위치 정보 갱신)
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
