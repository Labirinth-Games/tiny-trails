Esse é um jogo simples que tenta manter a temática e jogabilidade de um dungeon crawler.

[Backlog](https://www.notion.so/Backlog-1981d3dd7838807a984eefec58613f83?pvs=21)

[Referências de designer ](https://www.notion.so/Refer-ncias-de-designer-19a1d3dd783880329bd4d6f90ce3846c?pvs=21)

![alt text](images/image.png)

# Historia

Você é um aventureiro que tá em busca de matar o boss da dungeon e com isso ganhar fama e mostrar.

# Tipo de jogo

Dungeon crawler em turnos, para 1 jogador. que irá rodar no navegador.

# Mapa

O mapa é procedural, onde ele pode variar de tamanho e quantidades de salas, mas sempre na sala  mais distante onde o player nasceu será a sala do Boss.

O player só consegue ver a sala que ele está atualmente ou a que ele já passou, as demais salas ficam escondidas, só serão apresentadas ao jogador quando ele entrar pela porta.

## Tipos de Salas

Quando o mapa é gerado cada sala exceto a que o jogador nasceu tem um tipo, cada tipo implica o que irá aparecer para o jogador durante a gameplay. Todos os tipos são gerados proceduralmente no momento que o mapa é gerado

| Tipo | Descrição |
| --- | --- |
| Inimigo | Sala onde terá spawn de inimigos, ou seja, quando o jogador entrar ele se deparará com inimigos que terá que lutar. |
| Armadilha | Sala onde alguns tiles do chão pode causar dano, então é importante ter cuidado ao andar |
| Tesouro | Sala dedicada a espolios, ou seja, caso o jogador encontre uma sala dessa terá um bau onde pode encontrar diversos itens ou habilidades. |

## Sala Armadilha

A sala de armadilha eh basicamente alguns tiles do mapa causam dano quanso o jogador passa por cima.

O dano que armadilha causa vai danificando a defesa do jogador até que ele saia da sala, caso a defesa seja quebrada o jogador começa a tomar dano. 

# Jogador

Estatísticas que o jogador pode ter durante a gameplay.

| Stats | Descrição | Valor Base |
| --- | --- | --- |
| HP | Vida máxima do jogador. | 20 |
| Força | Quanto de dano ele aflige ao inimigo. | 5 |
| Defesa | Quantidade de dano que suporta | 3 |
| Movimento | Distancia que o jogador pode se mover, 1 significa 1 espaço no mapa (quadradinho) | 2 |
| Foco | Energia máxima que o jogador pode acumular durante sua jornada. representada por raios ⚡ | 3 |
| Distancia de ataque  | Distância que o jogador pode atacar  | Melee - 1
Range - 3 |

# Inimigo

Estatísticas que um inimigo pode ter durante a gameplay.

| Stats | Descrição |
| --- | --- |
| HP | Vida máxima do jogador. |
| Força | Quanto de dano ele aflige ao inimigo. |
| Defesa | Quantidade de dano que suporta |
| Movimento | Distancia que o jogador pode se mover, 1 significa 1 espaço no mapa (quadradinho) |
| Distância de ataque  | Distância que o inimigo pode atacar  |

# Combate

Durante a exploração o jogador pode combater com diversas ameaças ate encontrar o boss, o combate é por turno, ou seja, o jogador tem seu tempo de ataque e assim que finalizar o inimigo terá seu turno de ações.

## Foco

Para que a gameplay tenha um certo nível de gerenciamento, temos o foco representado por um ⚡, que é basicamente a energia usada para determinadas ações, ou seja, para que uma ação seja executada o jogador precisa ter a quantidade de foco(⚡) requerido para a ação. Ex: o jogador irá atacar, ele tem ⚡⚡, atacar custa ⚡, então ele pode atacar, já para usar um ação heroica ele precisa de ⚡⚡⚡, nesse caso ele não consegue usar essa ação.

O Foco só recarrega a partir de itens, ação de concentração e no fim do turno durante a batalha, onde ele recarrega +1 ⚡. 

Obs: se um jogador sair de uma batalha com 2 focos, e entrar em outra, o foco máximo do jogador não é recarregado, por tanto ele irá entrar numa batalha com a quantidade de foco que possui.

## Ações

O jogador possui 3 Pontos de ação (PA), isso quer dizer que ele só pode fazer 3 ações no seu turno. dentre as açoes temos:

| Ações | Descrição | Custo |
| --- | --- | --- |
| Mover | Ação usada para poder mover o jogador durante seu turno. | ⚡ |
| Atacar | Ação usada para infligir dano ao inimigo. | ⚡ |
| Item | Ação usada para usar um item do inventário. | ⚡ |
| Ação heroica | Ação que possui uma força maior que o ataque comum, podendo causar efeitos. | ⚡⚡⚡ |
| Antecipação | Antecipação é uma ação onde o jogador adiciona seleciona uma ação do inimigo e atribui uma ação dele, ou seja, um inimigo tem 2 ações, mas o jogador escolhe que na segunda ação do inimigo ele irá ter um ataque, quando o inimigo for fazer sua segunda ação ele irá executar a ação previamente atribuída, assim dando dinamismo nos ataques, recebendo bônus quando um ataque é bem sucedido. | ⚡⚡ |
| Concentrar | Ação que só pode ser usada 1 vez por turno, que recupera +1 ⚡(foco). | 
 |
| Defesa | Ação que pode ser usada para aumentar a quantidade de defesa do jogador em +2 🛡 | ⚡ |