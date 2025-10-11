// https://godotshaders.com/shader/cctv-old-security-camera-style-shader/

shader_type canvas_item;

uniform sampler2D screen_tex : hint_screen_texture;

uniform float exposure    : hint_range(0.2, 3.0) = 1.0;
uniform float contrast    : hint_range(0.5, 2.0) = 1.05;
uniform float saturation  : hint_range(0.0, 2.0) = 0.8;

uniform bool  enable_pixelate = true;
uniform float pixel_factor    : hint_range(1.0, 8.0) = 4.0;

uniform bool  enable_posterize = true;
uniform float color_levels     : hint_range(2.0, 32.0) = 8.0;
uniform float dither_strength  : hint_range(0.0, 1.0) = 0.35;

uniform float chrom_aberration : hint_range(0.0, 0.5) = 0.07;
uniform float vignette_strength: hint_range(0.0, 2.0) = 1.2;
uniform float grain_strength   : hint_range(0.0, 1.0) = 0.25;

uniform float tint_strength : hint_range(0.0, 1.0) = 0.25;

uniform float scanline_density : hint_range(50.0, 600.0) = 240.0;
uniform float scanline_strength: hint_range(0.0, 1.0) = 0.15;
uniform float glitch_strength  : hint_range(0.0, 1.0) = 0.12;
uniform float glitch_speed     : hint_range(0.0, 10.0) = 3.0;

uniform vec2 viewport_size_override = vec2(0.0, 0.0);

float rand(vec2 co){
    return fract(sin(dot(co.xy ,vec2(12.9898,78.233))) * 43758.5453);
}

float noise(vec2 p){
    vec2 i = floor(p);
    vec2 f = fract(p);
    float a = rand(i);
    float b = rand(i + vec2(1.0, 0.0));
    float c = rand(i + vec2(0.0, 1.0));
    float d = rand(i + vec2(1.0, 1.0));
    vec2 u = f*f*(3.0 - 2.0*f);
    return mix(a, b, u.x) + (c - a)* u.y * (1.0 - u.x) + (d - b) * u.x * u.y;
}

float frame_time(){
    return floor(TIME * 30.0) / 30.0;
}

vec3 two_color_palette(vec3 c, vec3 color_a, vec3 color_b, float strength){
    float lum = dot(c, vec3(0.333));
    vec3 mapped = mix(color_a, color_b, lum);
    return mix(c, mapped, strength);
}


void fragment(){
    vec2 uv = SCREEN_UV;
    vec2 px = SCREEN_PIXEL_SIZE;
    if (viewport_size_override.x > 0.0 && viewport_size_override.y > 0.0){
        px = 1.0 / viewport_size_override;
    }

    if (enable_pixelate){
        vec2 step_uv = px * pixel_factor;
        uv = floor(uv / step_uv) * step_uv + step_uv * 0.5;
    }

    float t = frame_time();

    float slice = floor(uv.y * scanline_density);
    float slice_noise = noise(vec2(slice * 0.1, floor(t * glitch_speed)));
    float h_off = (slice_noise - 0.5) * glitch_strength * 0.06;
    float burst = step(0.95, fract(noise(vec2(t * 0.3, 9.13))));
    h_off += (rand(vec2(t, slice))*2.0 - 1.0) * glitch_strength * 0.2 * burst;

    vec2 center = vec2(0.5);
    vec2 dir = uv - center;
    float dist = length(dir);
    float ca = chrom_aberration * dist * dist;

    vec3 col;
    col.r = texture(screen_tex, uv + vec2(h_off, 0.0) + dir * (ca)).r;
    col.g = texture(screen_tex, uv + vec2(h_off * 0.5, 0.0).g - dir * (ca)).g;
    col.b = texture(screen_tex, uv + vec2(h_off * 0.2, 0.0) - dir * (ca)).b;

    col *= exposure;

    if(enable_posterize){
        float d = rand(floor(uv / px) + vec2(t * 12.9898)) * dither_strength;
        vec3 q = floor(col * color_levels + d);
        col = q / max(color_levels - 1.0, 1.0);
    }

    col = two_color_palette(col, vec3(0.0,0.0,0.0), vec3(1.0,1.0,1.0), tint_strength);

    col = (col - 0.5) * contrast + 0.5;
    col = mix(vec3(dot(col, vec3(0.333))), col, saturation);

    float scan = sin((uv.y / px.y) * 3.14159 * scanline_density / 1000.0);
    col *= 1.0 - scan * scanline_strength;

    float vignette = smoothstep(0.35, 0.9, dist) * vignette_strength;
    col *= clamp(1.0 - vignette, 0.0, 1.0);

    float n = rand(uv * vec2(100.0, 100.0) + t * 37.0);
    col += (n - 0.5) * (px.x + px.y) * 2.0 * grain_strength;

    COLOR = vec4(clamp(col, 0.0, 1.0), 1.0);
}
