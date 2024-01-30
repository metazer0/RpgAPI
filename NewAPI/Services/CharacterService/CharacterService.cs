using AutoMapper;
using Microsoft.AspNetCore.Connections.Features;
using Microsoft.EntityFrameworkCore;
using NewAPI.Data;
using NewAPI.DTOs.Character;
using NewAPI.Models;

namespace NewAPI.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {

        private readonly IMapper _mapper;
        private readonly DataContext _context;

        public CharacterService(IMapper mapper, DataContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto character)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            var newCharacter = _mapper.Map<Character>(character);
            //newCharacter.Id = characters.Max(c => c.Id) + 1;
            //characters.Add(newCharacter);
            //serviceResponse.Data = characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
            _context.Characters.Add(newCharacter);
            await _context.SaveChangesAsync();
            var dbCharacters = await _context.Characters.ToListAsync();
            serviceResponse.Data = dbCharacters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();

            try
            {
                var deletedCharacter = await _context.Characters.FirstOrDefaultAsync(c => c.Id == id);

                if (deletedCharacter is null)
                {
                    throw new Exception($"Character with id {id} not found");
                }

                _context.Characters.Remove(deletedCharacter);

                await _context.SaveChangesAsync();

                serviceResponse.Data = await _context.Characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToListAsync();
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            var dbCharacters = await _context.Characters.ToListAsync();
            serviceResponse.Data = dbCharacters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
            return serviceResponse; 
        }

        public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();

            var dbCharacter = await _context.Characters.FirstOrDefaultAsync(c => c.Id == id);

            serviceResponse.Data = _mapper.Map<GetCharacterDto>(dbCharacter);

            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto character)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();

            try
            {
                var updatedCharacter = await _context.Characters.FirstOrDefaultAsync(c => c.Id == character.Id);

                if(updatedCharacter is null)
                {
                    throw new Exception($"Character with id {character.Id} not found");
                }

                updatedCharacter.Name = character.Name;
                updatedCharacter.HitPoints = character.HitPoints;
                updatedCharacter.Strength = character.Strength;
                updatedCharacter.Defense = character.Defense;
                updatedCharacter.Intelligence = character.Intelligence;
                updatedCharacter.Class = character.Class;

                await _context.SaveChangesAsync();
                serviceResponse.Data = _mapper.Map<GetCharacterDto>(updatedCharacter);
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }
    }
}
