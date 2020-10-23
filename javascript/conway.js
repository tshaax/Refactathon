async function runConway() {
    let populationBoard = [];
    let width = document.getElementById("width").value;
    let height = document.getElementById("height").value;
    let generations = document.getElementById("generations").value;
    let outputElement = document.getElementById("output");
    let canvas = document.getElementById("boardCanvas");
    let societyDied = false;

    if (width && height && generations) {
      populationBoard = createFirstGeneration(width, height);
      canvas.width = width * 4;
      canvas.length = length * 4;
      let canvasContext = canvas.getContext("2d");


      for (let generation = 1; generation <= generations; generation++) {
        outputElement.textContent = `Generation: ${generation}`;
        renderSocietyBoard(canvasContext, populationBoard);
        if (societyDied) {
          outputElement.innerHTML = `${outputElement.textContent} <br /> I guess that's the end of our little society `;
          break;
        }

        populationBoard = populateNextGeneration(populationBoard, width, height);
        await new Promise((resolve) => setTimeout(resolve, 1000));
      }
    }
  
    function renderSocietyBoard(canvasContext, currentGeneration) {
      let societyMembersAlive = 0;
      for (let xPosition = 0; xPosition < width; xPosition++) {
        for (let yPosition = 0; yPosition < height; yPosition++) {
          if (currentGeneration[xPosition][yPosition]) {
            canvasContext.fillStyle = "black";
            societyMembersAlive += 1;
          } else {
            canvasContext.fillStyle = "gainsboro";
          }
          canvasContext.fillRect(xPosition * 4, yPosition * 4, 4, 4);
        }
      }

      societyDied = societyMembersAlive === 0 ? true : false;
    }
  
    /**
     * Randomly generates the default generation society that the game's
     * future generations are based.
     */
    function createFirstGeneration(boardWidth, boardHeight) {
        let firstGeneration = [];
        let total = boardWidth * boardHeight;
        let ratio = (total * 40) / 100;
        for (let xPosition = 0; xPosition < boardWidth; xPosition++) {
            firstGeneration.push([]);
            for (let yPosition = 0; yPosition < boardHeight; yPosition++) {
                firstGeneration[xPosition].push(Math.floor(Math.random() * total + 1) < ratio);
            }
        }
        return firstGeneration;
    }
  
    /**
     * Generates a new population based on the conway rules
     */
    function populateNextGeneration(currentGeneration, boardWidth, boardHeight) {
      let nextGeneration = [];
      for (let xPosition = 0; xPosition < boardWidth; xPosition++) {
        nextGeneration.push([]);
        for (let yPosition = 0; yPosition < boardHeight; yPosition++) {
          let livingNeighbourCount = getNeighborsAlive(xPosition, yPosition, currentGeneration);
          nextGeneration[xPosition].push(
            (currentGeneration[xPosition][yPosition] && livingNeighbourCount === 2) ||
              livingNeighbourCount === 3
          );
        }
      }
      return nextGeneration;
    }
    
    function getNeighborsAlive(xPosition, yPosition, currentGeneration) {
      let neightborsAlive = 0;
      for (let neighborX = xPosition - 1; neighborX < xPosition + 2; neighborX++) {
        if (neighborOutOfBounds(neighborX, width)) {
          continue;
        }
        for (let neighborY = yPosition - 1; neighborY < yPosition + 2; neighborY++) {
          if (neighborIsSelf(xPosition, yPosition, neighborX, neighborY) || neighborOutOfBounds(neighborY, height)) {
            continue;
          }
          neightborsAlive += currentGeneration[neighborX][neighborY] ? 1 : 0;
        }
      }
      return neightborsAlive;
    }

    function neighborOutOfBounds(gridAxisValue, axisLimit) {
        return (gridAxisValue < 0 || gridAxisValue >= axisLimit) ? true : false;
    }

    function neighborIsSelf(xPosition, yPosition, neighborX, neighborY) {
        return (neighborX === xPosition && neighborY === yPosition) ? true : false;
    }
  }