async function runConway() {
    let width = document.getElementById("width").value;
    while (!width) {
        width = document.getElementById("width").value;
    }

    let height = document.getElementById("height").value;
    while (!height) {
        height = document.getElementById("height").value;
    }

    let generations = document.getElementById("generations").value;
    while (!generations) {
        generations = document.getElementById("generations").value;
    }
    let board = []

    let total = width * height;
    let ratio = (total * 40) / 100;

    for (let x = 0; x < width; x++) {
        board.push([]);
        for (let y = 0; y < height; y++) {
            board[x].push(Math.floor((Math.random() * total) + 1) < ratio);
        }
    }

    let outputElement = document.getElementById("output");
    let canvas = document.getElementById("boardCanvas");
    canvas.width = width * 4;
    canvas.length = length * 4;
    let canvasContext = canvas.getContext("2d");
    for (let i = 0; i <= generations; i++) {
        outputElement.textContent = "Generation: " + i;
        let societyDied = true;
        for (let x = 0; x < width; x++) {
            for (let y = 0; y < height; y++) {
                if (board[x][y]) {
                    canvasContext.fillStyle = "black";
                    societyDied = false;
                } else {
                    canvasContext.fillStyle = "gainsboro";
                }
                canvasContext.fillRect(x * 4, y * 4, 4, 4);
            }
        }
        if (societyDied) {
            outputElement.innerHTML = outputElement.textContent + "<br /> I guess that's the end of our little society";
            break;
        }

        let newBoard = [];
        for (let x = 0; x < width; x++) {
            newBoard.push([]);
            for (let y = 0; y < height; y++) {
                let livingNeighbourCount = 0;
                for (let xScan = x - 1; xScan < x + 2; xScan++) {
                    if (xScan < 0 || xScan >= width) {
                        continue;
                    }
                    for (let yScan = y - 1; yScan < y + 2; yScan++) {
                        if (xScan === x && yScan === y) {
                            continue;
                        }

                        if (yScan < 0 || yScan >= width) {
                            continue;
                        }

                        if (board[xScan][yScan]) {
                            livingNeighbourCount += 1;
                        }
                    }
                }
                newBoard[x].push((board[x][y] && livingNeighbourCount === 2) || livingNeighbourCount === 3);
            }
        }
        board = newBoard;
        await (new Promise(resolve => setTimeout(resolve, 1000)));
    }
};