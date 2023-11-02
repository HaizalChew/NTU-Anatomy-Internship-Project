using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PostProcess : MonoBehaviour
{
	public Renderer OutlinedObject;

	public Material WriteObject;

    
    public Material currentOutline;
	void Update()
	{
	
	}


	void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		//setup stuff
		var commands = new CommandBuffer();
		int selectionBuffer = Shader.PropertyToID("_SelectionBuffer");
		commands.GetTemporaryRT(selectionBuffer, source.descriptor);
		//render selection buffer
		commands.SetRenderTarget(selectionBuffer);
		commands.ClearRenderTarget(true, true, Color.clear);
		if (OutlinedObject != null)
		{
			commands.DrawRenderer(OutlinedObject, WriteObject);
		}
		//apply everything and clean up in commandbuffer
		commands.Blit(source, destination, currentOutline);
		commands.ReleaseTemporaryRT(selectionBuffer);

		
		//execute and clean up commandbuffer itself
		Graphics.ExecuteCommandBuffer(commands);
		commands.Dispose();
        Graphics.SetRenderTarget(destination);
	
        
        // CommandBuffer[] commands = new CommandBuffer[2];
        // int selectionBuffer = Shader.PropertyToID("_SelectionBuffer");
        // for(int i = 0; i < commands.Length; i++){
        //     commands[i] = new CommandBuffer();
        //     commands[i].GetTemporaryRT(selectionBuffer, source.descriptor);
        //     //render selection buffer
        //     commands[i].SetRenderTarget(selectionBuffer);
        //     commands[i].ClearRenderTarget(true, true, Color.clear);
        //     if (OutlinedObject[i] != null)
        //     {
        //         commands[i].DrawRenderer(OutlinedObject[i], WriteObject);
        //     }
        //     //apply everything and clean up in commandbuffer
        //     commands[i].Blit(source, destination, currentOutline[i]);
        //     commands[i].ReleaseTemporaryRT(selectionBuffer);
            
        //     //execute and clean up commandbuffer itself
        //     Graphics.ExecuteCommandBuffer(commands[i]);
        //     commands[i].Dispose();
        // }
	
    }
}
