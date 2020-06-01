/**
 * Copyright (c) 2017-present, PFW Contributors.
 *
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in
 * compliance with the License. You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software distributed under the License is
 * distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See
 * the License for the specific language governing permissions and limitations under the License.
 */

using UnityEngine;
using UnityEngine.VFX;

using PFW.Model.Armory.JsonContents;

namespace PFW.Units.Component.Weapon
{
    /// <summary>
    /// Represents a weapon firing in an arc.
    ///
    /// TODO rewrite or hopefully even entirely remove, this should not require
    /// a separate class.
    /// </summary>
    public sealed class Howitzer : IWeapon
    {
        private HowitzerConfig _data { get; }
        private float _reloadTimeLeft { get; set; }
        private AudioSource _audioSource { get; }

        // Where the shell spawns
        private Transform _barrelTip;

        private AudioClip _shotSound;
        private readonly VisualEffect _muzzleFlashEffect;
        private float _shotVolume;

        private readonly GameObject _shellArtPrefab;
        private readonly GameObject _shellPrefab;


        public Howitzer(
                HowitzerConfig data,
                AudioSource source,
                AudioClip shotSound,
                VisualEffect muzzleFlashEffect,
                Transform barrelTip,
                float shotVolume = 1.0F)
        {
            _data = data;
            _audioSource = source;
            _muzzleFlashEffect = muzzleFlashEffect;
            _shotSound = shotSound;
            _shotVolume = shotVolume;
            _barrelTip = barrelTip;
            _shellPrefab = Resources.Load<GameObject>("Shell");
            _shellArtPrefab = Resources.Load<GameObject>(_data.Shell);
        }

        private bool Shoot(TargetTuple target, bool isServer)
        {
            //  Vector3 start = new Vector3(ShotStarterPosition.position.x, ShotStarterPosition.position.y+0., ShotStarterPosition.position.z);

            GameObject shell = GameObject.Instantiate(
                    _shellPrefab,
                    _barrelTip.position,
                    _barrelTip.transform.rotation);
            GameObject.Instantiate(_shellArtPrefab, shell.transform);

            shell.GetComponent<ShellBehaviour>().Initialize(target.Position, _data.Velocity);

            _audioSource.PlayOneShot(_shotSound, _shotVolume);
            if (_muzzleFlashEffect != null)
            {
                _muzzleFlashEffect.transform.LookAt(target.Position);
                _muzzleFlashEffect.Play();
            }

            if (isServer) 
            {
                // TODO apply damage;
            }

            return true;
        }

        public void HandleUpdate()
        {
            if (_reloadTimeLeft > 0)
                _reloadTimeLeft -= Time.deltaTime;
        }

        public bool TryShoot(
                TargetTuple target,
                Vector3 displacement,
                bool isServer)
        {
            if (_reloadTimeLeft > 0)
                return false;

            _reloadTimeLeft = _data.SalvoReload;
            return Shoot(target, isServer);
        }
    }
}
