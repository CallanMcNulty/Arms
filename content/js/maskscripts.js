var shapes = ["heater", "cartouche", "swiss", "iberian", "lozenge", "none"];
var currentShape = $("input#special-form").val();
var shapeLoop = function(currentShape) {
  return shapes[Math.abs(currentShape % shapes.length)];
}

$(".shape").addClass(shapes[currentShape]);

$(".shape-right").click(function(){
  $(".shape").removeClass(shapeLoop(currentShape));
  currentShape++;
  $(".shape").addClass(shapeLoop(currentShape));
  $("input#special-form:text").val(Math.abs(currentShape % shapes.length));
})

$(".shape-left").click(function(){
  if(currentShape === 0) {
    currentShape = shapes.length;
  }

  $(".shape").removeClass(shapeLoop(currentShape));
  currentShape--;
  $(".shape").addClass(shapeLoop(currentShape));
  $("input#special-form:text").val(Math.abs(currentShape % shapes.length));
})

$("form#form1").submit(function(event){
  currentShape = $("input#special-form").val();
  $(".shape").addClass(shapes[currentShape]);
})
