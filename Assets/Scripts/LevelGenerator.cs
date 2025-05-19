using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;// 제네릭 컬렉션(Generic Collection) ex) List<int>을 사용하기 위해 필요한 네임스페이스 선언

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] GameObject chunkPrefab;
    [SerializeField] int StartingChunksAmount = 12;
    [SerializeField] Transform chunkParent;
    [SerializeField] float chunkLength = 10f;
    [SerializeField] float moveSpeed = 8f;

    //GameObject[] chunks = new GameObject[12];
    List<GameObject> chunks = new List<GameObject>();// chunks라는 이름의 GameObject 리스트를 새로 만들고, 지금부터 여기에 게임 오브젝트들을 추가하거나 관리하겠다는 뜻
                                                     // List<GameObject>는 GameObject를 여러 개 담을 수 있는 리스트 자료형, chunks는 이 리스트의 이름(변수명)
                                                     // new List<GameObject>()는 GameObject 리스트를 새로 생성(new)
    void Start()
    {
        SpawnStartingChunks();
    }
    void Update()//매 프레임마다 MoveChunks()를 호출해서 청크들을 앞으로 움직인다
    {
        MoveChunks();
    }
    void SpawnStartingChunks()
    {
        for (int i = 0; i < StartingChunksAmount; i++)// StartingChunksAmount만큼 청크를 생성하겠다는 뜻.(예: StartingChunksAmount = 12이면, 총 12개의 청크를 생성함)
        {
            SpawnChunk();
        }
    }

    private void SpawnChunk()
    {
        float spawnPositionZ = CalculateSpawnPositionZ();//spawnPositionZ는 현재 반복에서 청크가 Z축 어디에 놓일지 저장하는 변수

        Vector3 chunkSpawnPos = new Vector3(transform.position.x, transform.position.y, spawnPositionZ); // 현재 오브젝트(LevelGenerator)의 X, Y 좌표는 안바뀜
                                                                                                         // 즉 청크가 Z축으로 얼마나 떨어져서 생성될지를 결정하는 함수
        GameObject newChunk = Instantiate(chunkPrefab, chunkSpawnPos, Quaternion.identity, chunkParent);//chunkPrefab을 복제(instantiate) 해서 게임 화면에 생성
                                                                                                        //chunkParent는 저장공간, newChunk는 새로 생성한 청크(지형 블록) 오브젝트
                                                                                                        //chunkPrefab이라는 프리팹 오브젝트를 chunkSpawnPos 위치에 회전 없이(Quaternion.identity) 생성하고
                                                                                                        //생성된 오브젝트를 chunkParent라는 부모 오브젝트의 자식으로 설정한다.

        chunks.Add(newChunk);//Add(newChunk) → 청크 리스트에 새 청크를 등록
                             //Instantiate()로 새로 만든 청크 오브젝트(newChunk)를 chunks 리스트에 추가한다는 뜻
                             //chunks는 청크들을 담아두는 리스트
                             //이 리스트는 나중에 청크들을 움직이거나 제거할 때 사용
    }

    float CalculateSpawnPositionZ() //새 청크는 마지막 청크 위치 + 청크 길이 만큼 떨어진 위치에 생성돼서, 자연스럽게 기존 청크들 앞으로 이어짐 
    {
        float spawnPositionZ;
        if (chunks.Count == 0)
        {
            spawnPositionZ = transform.position.z;//첫 번째 청크(i == 0)는 기준 위치(transform.position.z)에 생성
                                                  //기준위치 = 이 코드가 붙어 있는 LevelGenerator 오브젝트의 위치
                                                  //이 스크립트를 가지고 있는 Level Generator라는 오브젝트이 위치는 유니티에서 (0,0,0)으로 설정했다.
                                                  //if (i == 1)이 되면 앞에 한칸이 비워져있다. why? if (i == 0)일때 spawnPositionZ = transform.position.z(기본 위치= 0,0,0 z는 0)이고,
                                                  //(else)일 때도  spawnPositionZ = transform.position.z + (0 * chunkLength) ----> spawnPositionZ = transform.position.z로 똑같기 때문
        }
        else
        {
            //spawnPositionZ = transform.position.z + (i * chunkLength);는 두 번째부터는, 기준 위치 + (청크 길이 × 청크 순서) 만큼 Z축에 더해서 멀리 떨어뜨려 놓음
            spawnPositionZ = chunks[chunks.Count - 1].transform.position.z + chunkLength;// 예를 들어 리스트 개수는 3인데 인덱스는 0, 1, 2 이렇게 세 개니까 마지막 청크 인덱스는 2 → chunks.Count - 1 이됨
                                                                                         // chunks.Count는 3 (청크가 3개 있다는 뜻,인덱스는 0, 1, 2 (3개니까 0부터 2까지 번호가 있음),
                                                                                         // 그래서 마지막 청크는 chunks[chunks.Count - 1] → chunks[2] 이게 마지막 청크를 가리킴, hunks[2]가 리스트 안에서 3번째 청크
        }

        return spawnPositionZ;
    }

    void MoveChunks()//청크를 매 프레임마다 뒤(플레이어 방향)로 이동시키고, 화면에서 사라진 청크는 제거하여 최적화
    {
        for (int i = 0; i < chunks.Count; i++)//i는 0부터 시작해서, i가 chunks 리스트의 요소 개수보다 작을 동안 i를 1씩 증가시키면서 반복한다"
                                              //chunks.Count는 현재 씬에 존재하는 청크 개수.
                                              //chunks.Count는 리스트에 들어있는 요소(게임 오브젝트)의 개수를 반환(현재 청크가 몇 개 있는지 알려줘)
                                              //ex) chunks에 5개의 오브젝트가 들어있으면, chunks.Count는 5

        {
            GameObject chunk = chunks[i];//chunks 리스트의 i번째 청크를 꺼내 변수 chunk에 저장
            chunks[i].transform.Translate(-transform.forward * (moveSpeed * Time.deltaTime));//모든 청크를, "프레임마다 Z축 뒤쪽(플레이어 방향)으로 moveSpeed 속도로 이동시켜라."
                                                                                             //즉, 배경이 계속 앞으로 밀려오게 만들어서 플레이어가 가만히 있어도 달리는 느낌을 주는 코드
                                                                                             //.transform.Translate(...)는 해당 오브젝트를 지정한 방향으로 이동시키는 함수
                                                                                             //transform.forward는 이 스크립트가 붙어 있는 오브젝트(LevelGenerator)**의 **Z+ 방향(앞쪽)을 의미
                                                                                             //-transform.forward는 그 반대 방향, 즉 Z- 방향(뒤쪽)을 의미(플레이어에게 다가오는 방향)
            
            if (chunk.transform.position.z <= Camera.main.transform.position.z - chunkLength)//이 조건은 청크가 화면에서 완전히 벗어났는지 확인하는 코드
                                                                                             // Camera.main.transform.position.z는 현재 메인 카메라(Z좌표), 즉 플레이어의 위치
                                                                                             // - chunkLength는 청크 하나의 길이만큼 더 뒤를 의미
                                                                                             //즉, "이 청크가 카메라보다 한 칸 이상 뒤에 있으면 제거해도 된다"는 의미
            {
                chunks.Remove(chunk);//조건에 일치하면 리스트에서 이 청크를 제거
                Destroy(chunk);//조건에 일치하면 실제로 청크 게임 오브젝트를 씬에서 삭제
                SpawnChunk();
            }
        }
    }

}



    

