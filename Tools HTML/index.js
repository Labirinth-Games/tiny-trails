const gridContainer = document.querySelector('.grid-container');
const resultado = document.getElementById('resultado');
const draw = document.getElementById('draw');
const map = Array.from(Array(4), () => new Array(4).fill(false));
const points = [];
const freeSpaces = [];
let last;

// Função para criar uma célula da grid
function criarCelula(y, x) {
    const cell = document.createElement('div');
    cell.classList.add('grid-item');
    cell.textContent = `(${y}, ${x})`;
    cell.addEventListener('click', () => {
        resultado.textContent = `Você clicou na célula: ${cell.textContent}`;
        map[y][x].cell.classList.add("red")
        map[y][x].isMarked = true
        
        let point;

        if (points.length === 0) point = { y: 0, x: 0, original: { y, x } }
        else point = { y: y - points[0].original.y, x: x - points[0].original.x, original: { y, x } }
        
        points.push(point);
        map[y][x].cell.textContent = `(${point.y}, ${point.x}) click ${points.length - 1}`
    });
    return cell;
}

function calc() {
    points.forEach(point => {
        const axis = [
            { x: 1, y: 0 },
            { x: -1, y: 0 },
            { x: 0, y: 1 },
            { x: 0, y: -1 },
            { x: 1, y: 1 },
            { x: -1, y: -1 },
            { x: 1, y: -1 },
            { x: -1, y: 1 },
        ]

        axis.forEach(pos => {
            const x = point.original.x + pos.x;
            const y = point.original.y + pos.y;
            const currentPos = map[y][x];

            if (!currentPos?.isMarked) {
                freeSpaces.push({ y: points[0].original.y - currentPos.y, x: points[0].original.x - currentPos.x, cell: currentPos.cell })
            }
        })
    });

    resultado.textContent = freeSpaces.map(f => `(${f.y},${f.x})`).join(", ")
    draw.textContent += points.map(f => `(${f.y},${f.x})`).join(", ")
    freeSpaces.forEach(f => {
        f.cell.classList.add("green")
        f.cell.textContent = `(${f.y},${f.x})`
    })
}

function copy(id) {
    navigator.clipboard.writeText(document.getElementById(id).innerHTML)
    
    let notify = document.createElement('p')
    notify.classList.add("notify")
    notify.textContent = "Copiado"

    document.body.prepend(notify)

    setTimeout(() => {
        notify.remove()
    }, 2000);
}

// Cria as 16x16 células
for (let y = 3; y >= 0; y--) {
    for (let x = 0; x < 4; x++) {
        const cell = criarCelula(y, x);
        map[y][x] = { y, x, cell }

        gridContainer.appendChild(cell);
    }
}