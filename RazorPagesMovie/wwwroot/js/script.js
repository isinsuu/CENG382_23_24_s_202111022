function elementVisibility() {
    var element = document.getElementById("itemToToggle");
    if (element.style.display === "none") {
      element.style.display = "inline";
    } else {
      element.style.display = "none";
    }
  }

  function showCalculator() {
    var form = document.getElementById('calculator');
    form.style.display = 'block';
  }

  function calculate() {
    var num1 = parseFloat(document.getElementById('num1').value);
    var num2 = parseFloat(document.getElementById('num2').value);

    if (isNaN(num1) || isNaN(num2)) {
      document.getElementById('result').innerText = "Please enter valid numbers.";
    } else {
      var sum = num1 + num2;
      document.getElementById('result').innerText = "Result: " + sum;
    }
  }