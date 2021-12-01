final tripleSum = (List<int> l, int i) => l[i - 2] + l[i - 1] + l[i];

void main(List<String> args) {
  final input = args[0].split("\n").map(int.parse).toList();

  var triples = <int>[];
  for (var i = 2; i < input.length; i++) triples.add(tripleSum(input, i));

  var increases = 0;
  for (var i = 1; i < triples.length; i++)
    if (triples[i] > triples[i - 1]) increases++;

  print(increases);
}
