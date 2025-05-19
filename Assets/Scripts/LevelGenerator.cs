using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;// ���׸� �÷���(Generic Collection) ex) List<int>�� ����ϱ� ���� �ʿ��� ���ӽ����̽� ����

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] GameObject chunkPrefab;
    [SerializeField] int StartingChunksAmount = 12;
    [SerializeField] Transform chunkParent;
    [SerializeField] float chunkLength = 10f;
    [SerializeField] float moveSpeed = 8f;

    //GameObject[] chunks = new GameObject[12];
    List<GameObject> chunks = new List<GameObject>();// chunks��� �̸��� GameObject ����Ʈ�� ���� �����, ���ݺ��� ���⿡ ���� ������Ʈ���� �߰��ϰų� �����ϰڴٴ� ��
                                                     // List<GameObject>�� GameObject�� ���� �� ���� �� �ִ� ����Ʈ �ڷ���, chunks�� �� ����Ʈ�� �̸�(������)
                                                     // new List<GameObject>()�� GameObject ����Ʈ�� ���� ����(new)
    void Start()
    {
        SpawnStartingChunks();
    }
    void Update()//�� �����Ӹ��� MoveChunks()�� ȣ���ؼ� ûũ���� ������ �����δ�
    {
        MoveChunks();
    }
    void SpawnStartingChunks()
    {
        for (int i = 0; i < StartingChunksAmount; i++)// StartingChunksAmount��ŭ ûũ�� �����ϰڴٴ� ��.(��: StartingChunksAmount = 12�̸�, �� 12���� ûũ�� ������)
        {
            SpawnChunk();
        }
    }

    private void SpawnChunk()
    {
        float spawnPositionZ = CalculateSpawnPositionZ();//spawnPositionZ�� ���� �ݺ����� ûũ�� Z�� ��� ������ �����ϴ� ����

        Vector3 chunkSpawnPos = new Vector3(transform.position.x, transform.position.y, spawnPositionZ); // ���� ������Ʈ(LevelGenerator)�� X, Y ��ǥ�� �ȹٲ�
                                                                                                         // �� ûũ�� Z������ �󸶳� �������� ���������� �����ϴ� �Լ�
        GameObject newChunk = Instantiate(chunkPrefab, chunkSpawnPos, Quaternion.identity, chunkParent);//chunkPrefab�� ����(instantiate) �ؼ� ���� ȭ�鿡 ����
                                                                                                        //chunkParent�� �������, newChunk�� ���� ������ ûũ(���� ���) ������Ʈ
                                                                                                        //chunkPrefab�̶�� ������ ������Ʈ�� chunkSpawnPos ��ġ�� ȸ�� ����(Quaternion.identity) �����ϰ�
                                                                                                        //������ ������Ʈ�� chunkParent��� �θ� ������Ʈ�� �ڽ����� �����Ѵ�.

        chunks.Add(newChunk);//Add(newChunk) �� ûũ ����Ʈ�� �� ûũ�� ���
                             //Instantiate()�� ���� ���� ûũ ������Ʈ(newChunk)�� chunks ����Ʈ�� �߰��Ѵٴ� ��
                             //chunks�� ûũ���� ��Ƶδ� ����Ʈ
                             //�� ����Ʈ�� ���߿� ûũ���� �����̰ų� ������ �� ���
    }

    float CalculateSpawnPositionZ() //�� ûũ�� ������ ûũ ��ġ + ûũ ���� ��ŭ ������ ��ġ�� �����ż�, �ڿ������� ���� ûũ�� ������ �̾��� 
    {
        float spawnPositionZ;
        if (chunks.Count == 0)
        {
            spawnPositionZ = transform.position.z;//ù ��° ûũ(i == 0)�� ���� ��ġ(transform.position.z)�� ����
                                                  //������ġ = �� �ڵ尡 �پ� �ִ� LevelGenerator ������Ʈ�� ��ġ
                                                  //�� ��ũ��Ʈ�� ������ �ִ� Level Generator��� ������Ʈ�� ��ġ�� ����Ƽ���� (0,0,0)���� �����ߴ�.
                                                  //if (i == 1)�� �Ǹ� �տ� ��ĭ�� ������ִ�. why? if (i == 0)�϶� spawnPositionZ = transform.position.z(�⺻ ��ġ= 0,0,0 z�� 0)�̰�,
                                                  //(else)�� ����  spawnPositionZ = transform.position.z + (0 * chunkLength) ----> spawnPositionZ = transform.position.z�� �Ȱ��� ����
        }
        else
        {
            //spawnPositionZ = transform.position.z + (i * chunkLength);�� �� ��°���ʹ�, ���� ��ġ + (ûũ ���� �� ûũ ����) ��ŭ Z�࿡ ���ؼ� �ָ� ����߷� ����
            spawnPositionZ = chunks[chunks.Count - 1].transform.position.z + chunkLength;// ���� ��� ����Ʈ ������ 3�ε� �ε����� 0, 1, 2 �̷��� �� ���ϱ� ������ ûũ �ε����� 2 �� chunks.Count - 1 �̵�
                                                                                         // chunks.Count�� 3 (ûũ�� 3�� �ִٴ� ��,�ε����� 0, 1, 2 (3���ϱ� 0���� 2���� ��ȣ�� ����),
                                                                                         // �׷��� ������ ûũ�� chunks[chunks.Count - 1] �� chunks[2] �̰� ������ ûũ�� ����Ŵ, hunks[2]�� ����Ʈ �ȿ��� 3��° ûũ
        }

        return spawnPositionZ;
    }

    void MoveChunks()//ûũ�� �� �����Ӹ��� ��(�÷��̾� ����)�� �̵���Ű��, ȭ�鿡�� ����� ûũ�� �����Ͽ� ����ȭ
    {
        for (int i = 0; i < chunks.Count; i++)//i�� 0���� �����ؼ�, i�� chunks ����Ʈ�� ��� �������� ���� ���� i�� 1�� ������Ű�鼭 �ݺ��Ѵ�"
                                              //chunks.Count�� ���� ���� �����ϴ� ûũ ����.
                                              //chunks.Count�� ����Ʈ�� ����ִ� ���(���� ������Ʈ)�� ������ ��ȯ(���� ûũ�� �� �� �ִ��� �˷���)
                                              //ex) chunks�� 5���� ������Ʈ�� ���������, chunks.Count�� 5

        {
            GameObject chunk = chunks[i];//chunks ����Ʈ�� i��° ûũ�� ���� ���� chunk�� ����
            chunks[i].transform.Translate(-transform.forward * (moveSpeed * Time.deltaTime));//��� ûũ��, "�����Ӹ��� Z�� ����(�÷��̾� ����)���� moveSpeed �ӵ��� �̵����Ѷ�."
                                                                                             //��, ����� ��� ������ �з����� ���� �÷��̾ ������ �־ �޸��� ������ �ִ� �ڵ�
                                                                                             //.transform.Translate(...)�� �ش� ������Ʈ�� ������ �������� �̵���Ű�� �Լ�
                                                                                             //transform.forward�� �� ��ũ��Ʈ�� �پ� �ִ� ������Ʈ(LevelGenerator)**�� **Z+ ����(����)�� �ǹ�
                                                                                             //-transform.forward�� �� �ݴ� ����, �� Z- ����(����)�� �ǹ�(�÷��̾�� �ٰ����� ����)
            
            if (chunk.transform.position.z <= Camera.main.transform.position.z - chunkLength)//�� ������ ûũ�� ȭ�鿡�� ������ ������� Ȯ���ϴ� �ڵ�
                                                                                             // Camera.main.transform.position.z�� ���� ���� ī�޶�(Z��ǥ), �� �÷��̾��� ��ġ
                                                                                             // - chunkLength�� ûũ �ϳ��� ���̸�ŭ �� �ڸ� �ǹ�
                                                                                             //��, "�� ûũ�� ī�޶󺸴� �� ĭ �̻� �ڿ� ������ �����ص� �ȴ�"�� �ǹ�
            {
                chunks.Remove(chunk);//���ǿ� ��ġ�ϸ� ����Ʈ���� �� ûũ�� ����
                Destroy(chunk);//���ǿ� ��ġ�ϸ� ������ ûũ ���� ������Ʈ�� ������ ����
                SpawnChunk();
            }
        }
    }

}



    

