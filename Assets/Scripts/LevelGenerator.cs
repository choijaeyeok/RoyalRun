using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] GameObject chunkPrefab;
    [SerializeField] int StartingChunksAmount = 12;
    [SerializeField] Transform chunkParent;
    [SerializeField] float chunkLength = 10f;
    void Start()
    {
        for (int i = 0; i < StartingChunksAmount; i++)// StartingChunksAmount만큼 청크를 생성하겠다는 뜻입니다.(예: StartingChunksAmount = 12이면, 총 12개의 청크를 생성함)
        {
            float spawnPositionZ;//현재 반복에서 청크가 Z축 어디에 놓일지 저장하는 변수

            if (i == 0)
            {
                spawnPositionZ = transform.position.z;//첫 번째 청크(i == 0)는 기준 위치(transform.position.z)에 생성
                                                      //기준위치 = 이 코드가 붙어 있는 LevelGenerator 오브젝트의 위치
                                                      //이 스크립트를 가지고 있는 Level Generator라는 오브젝트이 위치는 유니티에서 (0,0,0)으로 설정했다.
                                                      //if (i == 1)이 되면 앞에 한칸이 비워져있다. why? if (i == 0)일때 spawnPositionZ = transform.position.z(기본 위치= 0,0,0 z는 0)이고,
                                                      //i = 0(else)일 때도  spawnPositionZ = transform.position.z + (0 * chunkLength) ----> spawnPositionZ = transform.position.z로 똑같기 때문
            }
            else
            {
                spawnPositionZ = transform.position.z + (i * chunkLength);//두 번째부터는, 기준 위치 + (청크 길이 × 청크 순서) 만큼 Z축에 더해서 멀리 떨어뜨려 놓음
            }
            Vector3 chunkSpawnPos = new Vector3(transform.position.x, transform.position.y, spawnPositionZ); // 현재 오브젝트(LevelGenerator)의 X, Y 좌표는 안바뀜
                                                                                                             // 즉 청크가 Z축으로 얼마나 떨어져서 생성될지를 결정하는 함수
            Instantiate(chunkPrefab, chunkSpawnPos, Quaternion.identity, chunkParent);//chunkParent는 저장공간
                                                                                      //chunkPrefab이라는 프리팹 오브젝트를 chunkSpawnPos 위치에 회전 없이(Quaternion.identity) 생성하고
                                                                                      //생성된 오브젝트를 chunkParent라는 부모 오브젝트의 자식으로 설정한다.
        }
    }
        
}

    

