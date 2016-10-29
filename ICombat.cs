using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface ICombat
{
    //interface for all complat entities
    void Attack();
    void Magic();
    void Item();
    void Defend();
    int GetSpeed();
    string GetName();
    bool SetTurn(bool turn);
    void DoTurn();
    void TakeDamage(float damage);
}
