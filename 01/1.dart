void main(List<String> args) {
  final input = args[0].split("\n").map(int.parse).toList();

  var increases = 0;

  for (var i = 1; i < input.length; i++)
    if (input[i] > input[i - 1]) increases++;

  print(increases);
}
