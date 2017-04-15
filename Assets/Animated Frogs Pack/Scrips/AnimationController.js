#pragma strict




    var animator: Animator;
    var render: SkinnedMeshRenderer;
    
	var horizontalSpeed : float = 2.0;
	var verticalSpeed : float = 2.0;
	var scaleSpeed : float = 2.0;
	
	var lookAt: Transform;
	var animationNames: String[];
	
	var materials: Material[];
	
	private var initialScale: Vector3;
	private var s : float = -0.25;

	
	
function Start () 
{
  if (!animator) animator = GetComponent(Animator);
  initialScale = transform.localScale;
}

function Update ()
{
  if (Input.GetMouseButton(0))
   {
   	var h : float = horizontalSpeed * Input.GetAxis ("Mouse X");
    var v : float = verticalSpeed * Input.GetAxis ("Mouse Y");
	transform.Rotate (-v, -h, 0);
   }
   
   s += scaleSpeed * Input.GetAxis ("Mouse ScrollWheel");
   s = Mathf.Clamp(s, -0.7, 0.8);
   transform.localScale = initialScale  + initialScale*s;
   
   Camera.main.transform.position.y = Mathf.Lerp(Camera.main.transform.position.y, lookAt.position.y, 100*Time.deltaTime);
   Camera.main.transform.LookAt(lookAt);
   
}

function OnGUI() 
{
  var row: int = 0;
  var col: int = 0;
  
  for (var i:int = 0; i < animationNames.Length; i++)
    {
      if (col*100 > (Screen.width - 100)) 
        {
         row++;
         col = 0;
        }
        
	  if (GUI.Button(Rect(10 + col*100,10 + row*55,95,50), animationNames[i] )) animator.SetInteger("AnimationID", i);
	  col++;
	}
	
  for (i = 0; i < materials.Length; i++)
	if (GUI.Button(Rect(10 +i*105 ,Screen.height-55,100,50), materials[i].name )) render.material = materials[i];
}