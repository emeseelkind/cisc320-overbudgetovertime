#include <SDL2/SDL.h>
#include <SDL2/SDL_ttf.h>
#include <string>
#include <vector>
#include <iostream>

// Constants for window size
const int WINDOW_WIDTH = 800;
const int WINDOW_HEIGHT = 700;

// ASCII Art
const std::string bridgeArt = R"(
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

           /\                                        /\
          /  \                                      /  \
         /    \                                    /    \
        /      \                                  /      \
________/________\_____________________/_________/________\_________
|                       OVER BUDGET, OVER TIME                         |
|------------------------------------------------------------------------|
|                                                                        |
|------------------------------------------------------------------------|
|                   Warning: Bridge at maximum load!                     |
|______________________________________________________________________|

    ||            ||                   ||                   ||
    ||            ||                   ||                   ||
    ||            ||                   ||                   ||
    ||            ||                   ||                   ||
    ||____________||___________________||___________________||
 /////\\\\/////\\\\\\\\\\/////\\\\/////\\\\\\\\\\/////\\\\/////\\\\\
/////||||\\\\////||||||||||||\\\\/////||||||||||||\\\\////||||||||||\\\\
////||||||\\\\////||||||||||||||\\\\///||||||||||||||\\\\///||||||||||\\\\
            ================================================
                    THE BRIDGE IS COLLAPSING!
            ------------------------------------------------
)";

// Button structure
struct Button {
    SDL_Rect rect;
    std::string label;
    SDL_Color color;
    bool *state; // Pointer to the associated state (if applicable)
};

// Function to render text using SDL_ttf
void renderText(SDL_Renderer *renderer, TTF_Font *font, const std::string &text, int x, int y, SDL_Color color) {
    SDL_Surface *surface = TTF_RenderText_Blended_Wrapped(font, text.c_str(), color, WINDOW_WIDTH - 20);
    SDL_Texture *texture = SDL_CreateTextureFromSurface(renderer, surface);
    SDL_Rect dest = {x, y, surface->w, surface->h};
    SDL_RenderCopy(renderer, texture, nullptr, &dest);
    SDL_FreeSurface(surface);
    SDL_DestroyTexture(texture);
}

// Function to render buttons
void renderButton(SDL_Renderer *renderer, TTF_Font *font, const Button &button) {
    SDL_SetRenderDrawColor(renderer, button.color.r, button.color.g, button.color.b, button.color.a);
    SDL_RenderFillRect(renderer, &button.rect);
    SDL_SetRenderDrawColor(renderer, 0, 0, 0, 255); // Black border
    SDL_RenderDrawRect(renderer, &button.rect);

    // Render the label
    renderText(renderer, font, button.label, button.rect.x + 10, button.rect.y + 5, {255, 255, 255, 255});
}

// Main program
int main(int argc, char *argv[]) {
    // Initialize SDL and SDL_ttf
    if (SDL_Init(SDL_INIT_VIDEO) != 0 || TTF_Init() != 0) {
        std::cerr << "Error initializing SDL or SDL_ttf: " << SDL_GetError() << std::endl;
        return 1;
    }

    SDL_Window *window = SDL_CreateWindow("Bridge Game Launcher", SDL_WINDOWPOS_CENTERED, SDL_WINDOWPOS_CENTERED,
                                          WINDOW_WIDTH, WINDOW_HEIGHT, SDL_WINDOW_SHOWN);
    SDL_Renderer *renderer = SDL_CreateRenderer(window, -1, SDL_RENDERER_ACCELERATED | SDL_RENDERER_PRESENTVSYNC);

    // Load a font
    TTF_Font *font = TTF_OpenFont("assets/Consolas.ttf", 16); // Monospaced font for ASCII art
    if (!font) {
        std::cerr << "Error loading font: " << TTF_GetError() << std::endl;
        SDL_Quit();
        return 1;
    }

    // States
    bool masterVolume = true, music = true, gameSound = true;
    int difficulty = 1; // 0 = Easy, 1 = Medium, 2 = Hard

    // Define buttons
    std::vector<Button> buttons = {
        {{50, 600, 100, 30}, "Easy", {50, 50, 50, 255}, nullptr},
        {{200, 600, 100, 30}, "Medium", {2, 48, 32, 255}, nullptr}, // Default selected
        {{350, 600, 100, 30}, "Hard", {50, 50, 50, 255}, nullptr},
        {{500, 600, 120, 30}, "New Game", {50, 50, 50, 255}, nullptr},
        {{50, 650, 120, 30}, "Master: ON", {50, 50, 50, 255}, &masterVolume},
        {{200, 650, 120, 30}, "Music: ON", {50, 50, 50, 255}, &music},
        {{350, 650, 120, 30}, "Sound: ON", {50, 50, 50, 255}, &gameSound},
        {{500, 650, 120, 30}, "Load Game", {50, 50, 50, 255}, nullptr},
    };

    // Main loop
    bool running = true;
    SDL_Event e;
    while (running) {
        while (SDL_PollEvent(&e)) {
            if (e.type == SDL_QUIT) {
                running = false;
            } else if (e.type == SDL_MOUSEBUTTONDOWN) {
                int x = e.button.x, y = e.button.y;
                SDL_Point mousePoint = {x, y};
                for (auto &button : buttons) {
                    if (SDL_PointInRect(&mousePoint, &button.rect)) {
                        if (button.label == "Easy") {
                            difficulty = 0;
                            buttons[0].color = {2, 48, 32, 255}; // Highlight "Easy"
                            buttons[1].color = {50, 50, 50, 255};   // Unhighlight "Medium"
                            buttons[2].color = {50, 50, 50, 255};   // Unhighlight "Hard"
                        } else if (button.label == "Medium") {
                            difficulty = 1;
                            buttons[0].color = {50, 50, 50, 255};   // Unhighlight "Easy"
                            buttons[1].color = {2, 48, 32, 255}; // Highlight "Medium"
                            buttons[2].color = {50, 50, 50, 255};   // Unhighlight "Hard"
                        } else if (button.label == "Hard") {
                            difficulty = 2;
                            buttons[0].color = {50, 50, 50, 255};   // Unhighlight "Easy"
                            buttons[1].color = {50, 50, 50, 255};   // Unhighlight "Medium"
                            buttons[2].color = {2, 48, 32, 255}; // Highlight "Hard"
                        } else if (button.label.find("Master") != std::string::npos) {
                            masterVolume = !masterVolume;
                            button.label = masterVolume ? "Master: ON" : "Master: OFF";
                        } else if (button.label.find("Music") != std::string::npos) {
                            music = !music;
                            button.label = music ? "Music: ON" : "Music: OFF";
                        } else if (button.label.find("Sound") != std::string::npos) {
                            gameSound = !gameSound;
                            button.label = gameSound ? "Sound: ON" : "Sound: OFF";
                        } else if (button.label == "New Game") {
                            std::cout << "Starting a new game...\n";

                            // Build the command to launch Unity with arguments for a new game
                            std::string command = "\"Over Budget, Over Time.exe\" --newgame --difficulty=" + std::to_string(difficulty) +
                                                " --masterVolume=" + (masterVolume ? "1" : "0") +
                                                " --music=" + (music ? "1" : "0") +
                                                " --sound=" + (gameSound ? "1" : "0");

                            // Execute the Unity game
                            int result = std::system(command.c_str());
                            if (result != 0) {
                                std::cerr << "Failed to start the game. Error code: " << result << std::endl;
                            }

                            running = false; // Close the launcher
                        } else if (button.label == "Load Game") {
                            std::cout << "Loading game...\n";

                            // Build the command to launch Unity with arguments for loading a game
                            std::string command = "\"Over Budget, Over Time.exe\" --load --difficulty=" + std::to_string(difficulty) +
                                                " --masterVolume=" + (masterVolume ? "1" : "0") +
                                                " --music=" + (music ? "1" : "0") +
                                                " --sound=" + (gameSound ? "1" : "0");

                            // Execute the Unity game
                            int result = std::system(command.c_str());
                            if (result != 0) {
                                std::cerr << "Failed to start the game. Error code: " << result << std::endl;
                            }

                            running = false; // Close the launcher
                        }
                    }
                }
            }
        }

        // Render
        SDL_SetRenderDrawColor(renderer, 0, 0, 0, 255); // Black background
        SDL_RenderClear(renderer);

        renderText(renderer, font, bridgeArt, 10, 10, {200, 200, 200, 255}); // Render ASCII art

        for (const auto &button : buttons) {
            renderButton(renderer, font, button); // Render all buttons
        }

        SDL_RenderPresent(renderer);
    }

    // Cleanup
    TTF_CloseFont(font);
    SDL_DestroyRenderer(renderer);
    SDL_DestroyWindow(window);
    TTF_Quit();
    SDL_Quit();

    return 0;
}
