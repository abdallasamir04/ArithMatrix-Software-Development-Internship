using NumberGuessingGame.Services;
using NumberGuessingGame.UI;

// Program.cs is the application's entry point. Its only job is to
// construct the small number of top-level objects the application
// needs (here: the real random-number provider and the console menu)
// and start the program. It deliberately contains no game rules and
// no console I/O beyond starting the menu - all of that lives in
// Services/ and UI/ so it can be tested and reasoned about in isolation.

IRandomNumberProvider randomProvider = new RandomNumberProvider();
var menu = new ConsoleMenu(randomProvider);
menu.Run();
