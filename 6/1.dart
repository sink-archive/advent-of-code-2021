import 'dart:convert';
import 'dart:io';

String readAllStdin() {
  var working = "";
  while (true) {
    final line = stdin.readLineSync(encoding: utf8);
    if (line == null) {
      return working.substring(0, working.length - 1);
    }

    working += line + "\n";
  }
}

List<int> decreaseAllTimers(List<int> fish) => fish.map((f) => f - 1).toList();
int countNewFishNeeded(List<int> fish) => fish.where((f) => f == -1).length;
List<int> resetZeroFish(List<int> fish) =>
    fish.map((f) => f == -1 ? 6 : f).toList();

// addNewFish mutates in place, the others above return new lists, functional style
void addNewFish(List<int> fish, int count) =>
    fish.addAll(List.filled(count, 8));

void main() {
  final rawstdin = readAllStdin();

  var fish = rawstdin.split(",").map(int.parse).toList();

  // simulate 80 days
  for (var i = 1; i <= 80; i++) {
    // decrease every fish's timer
    fish = decreaseAllTimers(fish);
    // count new fish needed
    final newFishCount = countNewFishNeeded(fish);
    // reset any fish with timer at -1
    fish = resetZeroFish(fish);
    // add new fish
    addNewFish(fish, newFishCount);
  }

  print(fish.length);
}
