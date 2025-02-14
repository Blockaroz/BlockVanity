using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Graphics.CameraModifiers;

namespace BlockVanity.Core.Camera;

public class ContinuousShakeModifier : ICameraModifier
{
    public ContinuousShakeModifier(Vector2 bias, float strength, int time, int frequency = 4, string uniqueID = "")
    {
        _bias = bias;
        _strength = strength;
        _time = time;
        _frequency = frequency;
        _uniqueID = uniqueID;
    }

    private float _strength;
    private int _time;
    private int _frequency;

    private Vector2 _offset;
    private Vector2 _bias;
    private string _uniqueID;

    public string UniqueIdentity => _uniqueID;

    public bool Finished => _time <= 0;

    public void Update(ref CameraInfo cameraPosition)
    {
        if (_time % _frequency == 0)
            _offset = _offset * 0.1f + _bias + Main.rand.NextVector2Circular(1, 1) * _strength;

        cameraPosition.CameraPosition = cameraPosition.OriginalCameraPosition + _offset;
        _offset *= 1f / (_frequency / 2);
        _time--;
    }
}
