// IAttack.cs
public interface IAttack
{
    void GetAttackInput(bool fDown);   
    float GetSkillCooldown(); // ��ų ��Ÿ���� ��ȯ�ϴ� �޼���
    float GetLastSkillTime(); // ������ ��ų ��� �ð��� ��ȯ�ϴ� �޼���

}
