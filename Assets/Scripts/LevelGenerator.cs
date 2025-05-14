using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] GameObject chunkPrefab;
    [SerializeField] int StartingChunksAmount = 12;
    [SerializeField] Transform chunkParent;
    [SerializeField] float chunkLength = 10f;
    void Start()
    {
        for (int i = 0; i < StartingChunksAmount; i++)// StartingChunksAmount��ŭ ûũ�� �����ϰڴٴ� ���Դϴ�.(��: StartingChunksAmount = 12�̸�, �� 12���� ûũ�� ������)
        {
            float spawnPositionZ;//���� �ݺ����� ûũ�� Z�� ��� ������ �����ϴ� ����

            if (i == 0)
            {
                spawnPositionZ = transform.position.z;//ù ��° ûũ(i == 0)�� ���� ��ġ(transform.position.z)�� ����
                                                      //������ġ = �� �ڵ尡 �پ� �ִ� LevelGenerator ������Ʈ�� ��ġ
                                                      //�� ��ũ��Ʈ�� ������ �ִ� Level Generator��� ������Ʈ�� ��ġ�� ����Ƽ���� (0,0,0)���� �����ߴ�.
                                                      //if (i == 1)�� �Ǹ� �տ� ��ĭ�� ������ִ�. why? if (i == 0)�϶� spawnPositionZ = transform.position.z(�⺻ ��ġ= 0,0,0 z�� 0)�̰�,
                                                      //i = 0(else)�� ����  spawnPositionZ = transform.position.z + (0 * chunkLength) ----> spawnPositionZ = transform.position.z�� �Ȱ��� ����
            }
            else
            {
                spawnPositionZ = transform.position.z + (i * chunkLength);//�� ��°���ʹ�, ���� ��ġ + (ûũ ���� �� ûũ ����) ��ŭ Z�࿡ ���ؼ� �ָ� ����߷� ����
            }
            Vector3 chunkSpawnPos = new Vector3(transform.position.x, transform.position.y, spawnPositionZ); // ���� ������Ʈ(LevelGenerator)�� X, Y ��ǥ�� �ȹٲ�
                                                                                                             // �� ûũ�� Z������ �󸶳� �������� ���������� �����ϴ� �Լ�
            Instantiate(chunkPrefab, chunkSpawnPos, Quaternion.identity, chunkParent);//chunkParent�� �������
                                                                                      //chunkPrefab�̶�� ������ ������Ʈ�� chunkSpawnPos ��ġ�� ȸ�� ����(Quaternion.identity) �����ϰ�
                                                                                      //������ ������Ʈ�� chunkParent��� �θ� ������Ʈ�� �ڽ����� �����Ѵ�.
        }
    }
        
}

    

