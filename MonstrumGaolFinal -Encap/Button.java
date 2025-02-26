import greenfoot.*;  // Importing Greenfoot classes

/**
 * 
 * Button class for rock paper scissors
 * 
 * 
 */
public class Button extends Actor {
    private String move;  
    private GreenfootImage originalImage;  
    
    
    /**
     * setup for move
     * 
     */
    
    public Button(String move) {
        this.move = move;
        setImageForMove(move);
    }//end button
    
    
    /**
     * sets image based on the move used
     * 
     */
    
    
    private void setImageForMove(String move) {
        GreenfootImage image;

        if (move.equals("Rock")) {
            image = new GreenfootImage("rock.png");  
        } else if (move.equals("Paper")) {
            image = new GreenfootImage("paper.png");  
        } else if (move.equals("Scissors")) {
            image = new GreenfootImage("scissors.png");  
        } else {
            image = new GreenfootImage("default_image.png");  //in case bad
        }

        originalImage = image;
        setImage(originalImage);
        GreenfootImage myImage = getImage();
        int myNewHeight = (int) myImage.getHeight() * 3;
        int myNewWidth = (int) myImage.getWidth() * 3;
        myImage.scale(myNewWidth, myNewHeight);
    }//end setImageForMove
    
    
    
    /**
     * scales image if hovered over and checks for mouse
     * 
     */
    public void act() {
        // Check if the mouse is hovering over the button
        if (Greenfoot.mouseMoved(this)) {
            scaleImage(1.2);  
        } else if (Greenfoot.mouseMoved(null)) {
            scaleImage(1.0);  
        }

        // Check if the button is clicked by the mouse
        if (Greenfoot.mouseClicked(this)) {
            ((GameBoard) getWorld()).setResult(determineWinner(move));
        }
    }//end act
    
    
    
    /**
     * image scaler in case the other one fails
     * 
     */
    private void scaleImage(double scaleFactor) {
        int newWidth = (int) (originalImage.getWidth() * scaleFactor);
        int newHeight = (int) (originalImage.getHeight() * scaleFactor);
        GreenfootImage scaledImage = new GreenfootImage(originalImage);
        scaledImage.scale(newWidth, newHeight);
        setImage(scaledImage);
    }//end scaleImage

    /**
     * game outcome
     * 
     */
    private String determineWinner(String playerMove) {
        String[] moves = {"Rock", "Paper", "Scissors"};
        String computerMove = moves[Greenfoot.getRandomNumber(3)];

        ComputerMove computerMoveActor = new ComputerMove();
        computerMoveActor.setMoveImage(computerMove);

        int computerX = 350;  
        int computerY = 350;  
        getWorld().addObject(computerMoveActor, computerX, computerY);

        // Determine the winner
        if (playerMove.equals(computerMove)) {
            return "Draw! Both chose " + playerMove;
        } else if ((playerMove.equals("Rock") && computerMove.equals("Scissors")) ||
                   (playerMove.equals("Paper") && computerMove.equals("Rock")) ||
                   (playerMove.equals("Scissors") && computerMove.equals("Paper"))) {
            return "You Win! " + playerMove + " beats " + computerMove;
        } else {
            return "You Lose! " + computerMove + " beats " + playerMove;
        }
    }//end determineWinner

    /**
     * getters and setters
     * 
     */

    public String getMove() {
        return move;
    }// end getMove

    public void setMove(String move) {
        this.move = move;
        setImageForMove(move);
    }//end setMove

    public GreenfootImage getOriginalImage() {
        return originalImage;
    }// end getOriginalImage

    public void setOriginalImage(GreenfootImage originalImage) {
        this.originalImage = originalImage;
        setImage(originalImage);
    }// end setOriginalImage
}//end class button
